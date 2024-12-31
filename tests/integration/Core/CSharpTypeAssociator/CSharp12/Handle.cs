namespace Paraminter.Associating.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.CSharp.Type.Corus.Errors.Commands;
using Paraminter.Associating.CSharp.Type.Corus.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

using System;

using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public async Task MethodInvocation_PairsAll()
    {
        var source = """
            public class Foo
            {
                public void Source()
                {
                    A<int, string, bool>();
                }

                public void Target<T1, T2, T3>() { }
            }
            """;

        var compilation = CompilationFactory.Create(source);

        var target = compilation.GetTypeByMetadataName("Foo")!.GetMembers().OfType<IMethodSymbol>().Single(static (symbol) => symbol.Name == "Target");

        var parameters = target.TypeParameters;
        var syntacticArguments = (await compilation.SyntaxTrees[0].GetRootAsync(CancellationToken.None)).DescendantNodes().OfType<TypeArgumentListSyntax>().Single().Arguments;

        Mock<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns(parameters);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns(syntacticArguments);

        await Target(commandMock.Object, CancellationToken.None);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>(), It.IsAny<CancellationToken>()), Times.Never());

        Fixture.PairerMock.Verify(PairArgumentExpression(parameters[0], syntacticArguments[0], It.IsAny<CancellationToken>()), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameters[1], syntacticArguments[1], It.IsAny<CancellationToken>()), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameters[2], syntacticArguments[2], It.IsAny<CancellationToken>()), Times.Once());
        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
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
