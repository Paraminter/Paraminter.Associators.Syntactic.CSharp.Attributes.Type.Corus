namespace Paraminter.Associators.Syntactic.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.Associators.Syntactic.CSharp.Type.Corus.Queries;
using Paraminter.Associators.Syntactic.CSharp.Type.Queries.Collectors;
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
        private readonly IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, ISyntacticCSharpTypeInvocationDataAssociatorQueryResponseCollector> Sut;

        public Fixture(
            IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, ISyntacticCSharpTypeInvocationDataAssociatorQueryResponseCollector> sut)
        {
            Sut = sut;
        }

        IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, ISyntacticCSharpTypeInvocationDataAssociatorQueryResponseCollector> IFixture.Sut => Sut;
    }
}
