namespace Paraminter.Associators.Syntactic.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.Associators.Syntactic.CSharp.Type.Corus.Queries;
using Paraminter.Associators.Syntactic.CSharp.Type.Queries.Collectors;
using Paraminter.Queries.Handlers;

internal interface IFixture
{
    public abstract IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, ISyntacticCSharpTypeInvocationDataAssociatorQueryResponseCollector> Sut { get; }
}
