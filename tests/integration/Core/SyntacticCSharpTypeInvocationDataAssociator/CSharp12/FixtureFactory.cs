namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Collectors;
using Paraminter.Queries.Handlers;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        SyntacticCSharpTypeInvocationDataAssociator sut = new();

        return new Fixture(sut);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly IQueryHandler<IAssociateArgumentsQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector> Sut;

        public Fixture(
            IQueryHandler<IAssociateArgumentsQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector> sut)
        {
            Sut = sut;
        }

        IQueryHandler<IAssociateArgumentsQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector> IFixture.Sut => Sut;
    }
}
