namespace Paraminter.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Commands;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Handlers;
using Paraminter.Queries.Invalidation.Commands;

using System;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void NullQuery_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullQueryResponseHandler_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void DifferentNumberOfParametersAndSyntacticArguments_Invalidates()
    {
        Mock<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup(static (query) => query.Data.Parameters).Returns([]);
        queryMock.Setup(static (query) => query.Data.SyntacticArguments).Returns([SyntaxFactory.ParseTypeName("int")]);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.Invalidator.Handle(It.IsAny<IInvalidateQueryResponseCommand>()), Times.Once());
    }

    [Fact]
    public void NoParametersOrSyntacticArguments_AddsNone()
    {
        Mock<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup(static (query) => query.Data.Parameters).Returns([]);
        queryMock.Setup(static (query) => query.Data.SyntacticArguments).Returns([]);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.Invalidator.Handle(It.IsAny<IInvalidateQueryResponseCommand>()), Times.Never());
        queryResponseHandlerMock.Verify(static (handler) => handler.AssociationCollector.Handle(It.IsAny<IAddCSharpTypeAssociationCommand>()), Times.Never());
    }

    [Fact]
    public void SameNumberOfParametersAndArguments_AddsAllPairwise()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();

        var syntacticArgument1 = SyntaxFactory.ParseTypeName("int");
        var syntacticArgument2 = SyntaxFactory.ParseTypeName("float");

        Mock<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Parameters).Returns([parameter1, parameter2]);
        queryMock.Setup((query) => query.Data.SyntacticArguments).Returns([syntacticArgument1, syntacticArgument2]);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.Invalidator.Handle(It.IsAny<IInvalidateQueryResponseCommand>()), Times.Never());
        queryResponseHandlerMock.Verify(static (handler) => handler.AssociationCollector.Handle(It.IsAny<IAddCSharpTypeAssociationCommand>()), Times.Exactly(2));
        queryResponseHandlerMock.Verify(AssociationExpression(parameter1, syntacticArgument1), Times.Once());
        queryResponseHandlerMock.Verify(AssociationExpression(parameter2, syntacticArgument2), Times.Once());
    }

    private static Expression<Action<IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler>> AssociationExpression(
        ITypeParameterSymbol parameter,
        TypeSyntax syntacticArgument)
    {
        return (handler) => handler.AssociationCollector.Handle(It.Is(MatchAssociationCommand(parameter, syntacticArgument)));
    }

    private static Expression<Func<IAddCSharpTypeAssociationCommand, bool>> MatchAssociationCommand(
        ITypeParameterSymbol parameter,
        TypeSyntax syntacticArgument)
    {
        return (command) => ReferenceEquals(command.Parameter, parameter) && ReferenceEquals(command.SyntacticArgument, syntacticArgument);
    }

    private void Target(
        IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData> query,
        IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler queryResponseHandler)
    {
        Fixture.Sut.Handle(query, queryResponseHandler);
    }
}
