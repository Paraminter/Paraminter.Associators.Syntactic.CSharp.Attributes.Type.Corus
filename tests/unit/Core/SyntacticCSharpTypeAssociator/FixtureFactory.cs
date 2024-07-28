namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Collectors;
using Paraminter.Queries.Handlers;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        SyntacticCSharpTypeAssociator sut = new();

        return new Fixture(sut);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingAssociateSyntacticCSharpTypeQueryResponseCollector> Sut;

        public Fixture(
            IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingAssociateSyntacticCSharpTypeQueryResponseCollector> sut)
        {
            Sut = sut;
        }

        IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingAssociateSyntacticCSharpTypeQueryResponseCollector> IFixture.Sut => Sut;
    }
}
