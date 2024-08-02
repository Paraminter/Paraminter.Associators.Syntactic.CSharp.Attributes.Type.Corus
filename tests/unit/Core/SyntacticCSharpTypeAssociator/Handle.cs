namespace Paraminter.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.CSharp.Type.Commands;
using Paraminter.CSharp.Type.Corus.Commands;

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
    public void DifferentNumberOfParametersAndSyntacticArguments_RecordsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns([SyntaxFactory.ParseTypeName("int")]);

        Target(commandMock.Object);

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordCSharpTypeAssociationCommand>()), Times.Never());
    }

    [Fact]
    public void NoParametersOrSyntacticArguments_RecordsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns([]);

        Target(commandMock.Object);

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordCSharpTypeAssociationCommand>()), Times.Never());
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

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordCSharpTypeAssociationCommand>()), Times.Exactly(2));
        Fixture.RecorderMock.Verify(RecordExpression(parameter1, syntacticArgument1), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameter2, syntacticArgument2), Times.Once());
    }

    private static Expression<Action<ICommandHandler<IRecordCSharpTypeAssociationCommand>>> RecordExpression(
        ITypeParameterSymbol parameter,
        TypeSyntax syntacticArgument)
    {
        return (recorder) => recorder.Handle(It.Is(MatchRecordCommand(parameter, syntacticArgument)));
    }

    private static Expression<Func<IRecordCSharpTypeAssociationCommand, bool>> MatchRecordCommand(
        ITypeParameterSymbol parameter,
        TypeSyntax syntacticArgument)
    {
        return (command) => ReferenceEquals(command.Parameter, parameter) && ReferenceEquals(command.SyntacticArgument, syntacticArgument);
    }

    private void Target(
        IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
