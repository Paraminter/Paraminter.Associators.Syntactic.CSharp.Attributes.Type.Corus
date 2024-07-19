namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Collectors;
using Paraminter.Queries.Handlers;

using System;

/// <summary>Associates syntactic C# type arguments.</summary>
public sealed class SyntacticCSharpTypeInvocationDataAssociator
    : IQueryHandler<IAssociateArgumentsQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector>
{
    /// <summary>Instantiates a <see cref="SyntacticCSharpTypeInvocationDataAssociator"/>, associating syntactic C# type arguments.</summary>
    public SyntacticCSharpTypeInvocationDataAssociator() { }

    void IQueryHandler<IAssociateArgumentsQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector>.Handle(
        IAssociateArgumentsQuery<IUnassociatedSyntacticCSharpTypeInvocationData> query,
        IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector queryResponseCollector)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (queryResponseCollector is null)
        {
            throw new ArgumentNullException(nameof(queryResponseCollector));
        }

        if (query.Data.Parameters.Count != query.Data.SyntacticArguments.Count)
        {
            queryResponseCollector.Invalidator.Invalidate();

            return;
        }

        for (var i = 0; i < query.Data.Parameters.Count; i++)
        {
            var parameter = query.Data.Parameters[i];
            var argumentData = query.Data.SyntacticArguments[i];

            queryResponseCollector.Associations.Add(parameter, argumentData);
        }
    }
}
