namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Collectors;
using Paraminter.Queries.Handlers;

internal interface IFixture
{
    public abstract IQueryHandler<IAssociateArgumentsQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector> Sut { get; }
}
