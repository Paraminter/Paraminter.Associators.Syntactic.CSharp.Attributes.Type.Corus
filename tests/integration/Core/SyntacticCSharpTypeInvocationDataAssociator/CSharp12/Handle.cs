namespace Paraminter.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Collectors;

using System.Linq;

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
                public void Invoke()
                {
                    Method<int, string, bool>();
                }

                public void Method<T1, T2, T3>() { }
            }
            """;

        var compilation = CompilationFactory.Create(source);

        var type = compilation.GetTypeByMetadataName("Foo")!;
        var method = type.GetMembers().OfType<IMethodSymbol>().Single((symbol) => symbol.Name == "Method");

        var syntaxTree = compilation.SyntaxTrees[0];

        var invokeMethod = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().Single((method) => method.Identifier.Text == "Invoke");
        var methodInvocation = invokeMethod.DescendantNodes().OfType<InvocationExpressionSyntax>().Single();
        var syntacticArguments = methodInvocation.DescendantNodes().OfType<TypeArgumentListSyntax>().Single().Arguments;

        Mock<IAssociateArgumentsQuery<IUnassociatedSyntacticCSharpTypeInvocationData>> queryMock = new();
        Mock<IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector> queryResponseCollectorMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Parameters).Returns(method.TypeParameters);
        queryMock.Setup((query) => query.Data.SyntacticArguments).Returns(syntacticArguments);

        Target(queryMock.Object, queryResponseCollectorMock.Object);

        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(method.TypeParameters[0], syntacticArguments[0]), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(method.TypeParameters[1], syntacticArguments[1]), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(method.TypeParameters[2], syntacticArguments[2]), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(It.IsAny<ITypeParameterSymbol>(), It.IsAny<TypeSyntax>()), Times.Exactly(3));
    }

    private void Target(
        IAssociateArgumentsQuery<IUnassociatedSyntacticCSharpTypeInvocationData> query,
        IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector queryResponseCollector)
    {
        Fixture.Sut.Handle(query, queryResponseCollector);
    }
}
