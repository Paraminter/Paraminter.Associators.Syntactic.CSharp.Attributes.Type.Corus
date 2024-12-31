namespace Paraminter.Associating.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.CSharp.Type.Corus.Errors.Commands;
using Paraminter.Associating.CSharp.Type.Corus.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public async Task NullCommand_ThrowsArgumentNullException()
    {
        var result = await Record.ExceptionAsync(() => Target(null!, CancellationToken.None));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public async Task DifferentNumberOfParametersAndSyntacticArguments_HandlesError()
    {
        Mock<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns([SyntaxFactory.ParseTypeName("int")]);

        await Target(commandMock.Object, CancellationToken.None);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task NoParametersOrSyntacticArguments_PairsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([]);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns([]);

        await Target(commandMock.Object, CancellationToken.None);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>(), It.IsAny<CancellationToken>()), Times.Never());

        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task SameNumberOfParametersAndArguments_PairsAll()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();

        var syntacticArgument1 = SyntaxFactory.ParseTypeName("int");
        var syntacticArgument2 = SyntaxFactory.ParseTypeName("float");

        Mock<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns([parameter1, parameter2]);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns([syntacticArgument1, syntacticArgument2]);

        await Target(commandMock.Object, CancellationToken.None);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>(), It.IsAny<CancellationToken>()), Times.Never());

        Fixture.PairerMock.Verify(PairArgumentExpression(parameter1, syntacticArgument1, It.IsAny<CancellationToken>()), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameter2, syntacticArgument2, It.IsAny<CancellationToken>()), Times.Once());
        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    private static Expression<Func<ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>, Task>> PairArgumentExpression(
        ITypeParameterSymbol parameter,
        TypeSyntax syntacticArgument,
        CancellationToken cancellationToken)
    {
        return (handler) => handler.Handle(It.Is(MatchPairArgumentCommand(parameter, syntacticArgument)), cancellationToken);
    }

    private static Expression<Func<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>, bool>> MatchPairArgumentCommand(
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

    private async Task Target(
        IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData> command,
        CancellationToken cancellationToken)
    {
        await Fixture.Sut.Handle(command, cancellationToken);
    }
}
