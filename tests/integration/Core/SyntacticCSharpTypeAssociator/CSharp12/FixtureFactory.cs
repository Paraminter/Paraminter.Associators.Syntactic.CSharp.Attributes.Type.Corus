namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Handlers;
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
        private readonly IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler> Sut;

        public Fixture(
            IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler> sut)
        {
            Sut = sut;
        }

        IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler> IFixture.Sut => Sut;
    }
}
