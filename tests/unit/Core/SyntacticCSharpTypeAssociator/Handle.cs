namespace Paraminter.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Models;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;

using System;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void NullCommand_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void DifferentNumberOfParametersAndSyntacticArguments_Invalidates()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns([SyntaxFactory.ParseTypeName("int")]);

        Target(commandMock.Object);

        Fixture.InvalidatorMock.Verify(static (invalidator) => invalidator.Handle(It.IsAny<IInvalidateArgumentAssociationsRecordCommand>()), Times.AtLeastOnce());
    }

    [Fact]
    public void NoParametersOrSyntacticArguments_RecordsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns([]);

        Target(commandMock.Object);

        Fixture.InvalidatorMock.Verify(static (invalidator) => invalidator.Handle(It.IsAny<IInvalidateArgumentAssociationsRecordCommand>()), Times.Never());

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>()), Times.Never());
    }

    [Fact]
    public void SameNumberOfParametersAndArguments_AddsAllPairwise()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();

        var syntacticArgument1 = SyntaxFactory.ParseTypeName("int");
        var syntacticArgument2 = SyntaxFactory.ParseTypeName("float");

        Mock<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([parameter1, parameter2]);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns([syntacticArgument1, syntacticArgument2]);

        Target(commandMock.Object);

        Fixture.InvalidatorMock.Verify(static (invalidator) => invalidator.Handle(It.IsAny<IInvalidateArgumentAssociationsRecordCommand>()), Times.Never());

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>()), Times.Exactly(2));
        Fixture.RecorderMock.Verify(RecordExpression(parameter1, syntacticArgument1), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameter2, syntacticArgument2), Times.Once());
    }

    private static Expression<Action<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>>> RecordExpression(
        ITypeParameterSymbol parameter,
        TypeSyntax syntacticArgument)
    {
        return (recorder) => recorder.Handle(It.Is(MatchRecordCommand(parameter, syntacticArgument)));
    }

    private static Expression<Func<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>, bool>> MatchRecordCommand(
        ITypeParameterSymbol parameterSymbol,
        TypeSyntax syntacticArgument)
    {
        return (command) => MatchParameter(parameterSymbol, command.Parameter) && MatchArgumentData(syntacticArgument, command.ArgumentData);
    }

    private static bool MatchParameter(
        ITypeParameterSymbol parameterSymbol,
        ITypeParameter parameter)
    {
        return ReferenceEquals(parameterSymbol, parameter.Symbol);
    }

    private static bool MatchArgumentData(
        TypeSyntax syntacticArgument,
        ICSharpTypeArgumentData argumentData)
    {
        return ReferenceEquals(syntacticArgument, argumentData.SyntacticArgument);
    }

    private void Target(
        IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
