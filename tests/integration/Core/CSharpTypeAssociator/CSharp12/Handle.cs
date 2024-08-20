namespace Paraminter.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Errors.Commands;
using Paraminter.CSharp.Type.Corus.Models;
using Paraminter.Parameters.Type.Models;

using System;

using System.Linq;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void MethodInvocation_AssociatesAll()
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
        var syntacticArguments = compilation.SyntaxTrees[0].GetRoot().DescendantNodes().OfType<TypeArgumentListSyntax>().Single().Arguments;

        Mock<IAssociateAllArgumentsCommand<IAssociateAllSyntacticCSharpTypeArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns(parameters);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns(syntacticArguments);

        Target(commandMock.Object);

        Fixture.ErrorHandlerMock.Verify(static (handler) => handler.DifferentNumberOfArgumentsAndParameters.Handle(It.IsAny<IHandleDifferentNumberOfArgumentsAndParametersCommand>()), Times.Never());

        Fixture.IndividualAssociatorMock.Verify(static (associator) => associator.Handle(It.IsAny<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>()), Times.Exactly(3));
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameters[0], syntacticArguments[0]), Times.Once());
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameters[1], syntacticArguments[1]), Times.Once());
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameters[2], syntacticArguments[2]), Times.Once());
    }

    private static Expression<Action<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>>> AssociateIndividualExpression(
        ITypeParameterSymbol parameter,
        TypeSyntax syntacticArgument)
    {
        return (associator) => associator.Handle(It.Is(MatchAssociateIndividualCommand(parameter, syntacticArgument)));
    }

    private static Expression<Func<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>, bool>> MatchAssociateIndividualCommand(
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
        IAssociateAllArgumentsCommand<IAssociateAllSyntacticCSharpTypeArgumentsData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
