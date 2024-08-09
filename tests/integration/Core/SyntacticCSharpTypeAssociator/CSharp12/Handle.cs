namespace Paraminter.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Models;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;

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

        Mock<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Parameters).Returns(parameters);
        commandMock.Setup(static (command) => command.Data.SyntacticArguments).Returns(syntacticArguments);

        Target(commandMock.Object);

        Fixture.InvalidatorMock.Verify(static (invalidator) => invalidator.Handle(It.IsAny<IInvalidateArgumentAssociationsRecordCommand>()), Times.Never());

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>()), Times.Exactly(3));
        Fixture.RecorderMock.Verify(RecordExpression(parameters[0], syntacticArguments[0]), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameters[1], syntacticArguments[1]), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameters[2], syntacticArguments[2]), Times.Once());
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
