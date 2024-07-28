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
                public void Source()
                {
                    A<int, string, bool>();
                }

                public void Target<T1, T2, T3>() { }
            }
            """;

        var compilation = CompilationFactory.Create(source);

        var target = compilation.GetTypeByMetadataName("Foo")!.GetMembers().OfType<IMethodSymbol>().Single((symbol) => symbol.Name == "Target");

        var syntacticArguments = compilation.SyntaxTrees[0].GetRoot().DescendantNodes().OfType<TypeArgumentListSyntax>().Single().Arguments;

        Mock<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>> queryMock = new();
        Mock<IInvalidatingAssociateSyntacticCSharpTypeQueryResponseCollector> queryResponseCollectorMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Parameters).Returns(target.TypeParameters);
        queryMock.Setup((query) => query.Data.SyntacticArguments).Returns(syntacticArguments);

        Target(queryMock.Object, queryResponseCollectorMock.Object);

        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(target.TypeParameters[0], syntacticArguments[0]), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(target.TypeParameters[1], syntacticArguments[1]), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(target.TypeParameters[2], syntacticArguments[2]), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(It.IsAny<ITypeParameterSymbol>(), It.IsAny<TypeSyntax>()), Times.Exactly(3));

        queryResponseCollectorMock.Verify((collector) => collector.Invalidator.Invalidate(), Times.Never());
    }

    private void Target(
        IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData> query,
        IInvalidatingAssociateSyntacticCSharpTypeQueryResponseCollector queryResponseCollector)
    {
        Fixture.Sut.Handle(query, queryResponseCollector);
    }
}
