namespace Paraminter.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.CSharp.Type.Commands;
using Paraminter.CSharp.Type.Corus.Commands;

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

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordCSharpTypeAssociationCommand>()), Times.Exactly(3));
        Fixture.RecorderMock.Verify(RecordExpression(parameters[0], syntacticArguments[0]), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameters[1], syntacticArguments[1]), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameters[2], syntacticArguments[2]), Times.Once());
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
