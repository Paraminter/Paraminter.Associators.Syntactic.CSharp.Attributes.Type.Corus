namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Collectors;
using Paraminter.Queries.Handlers;

using System;

/// <summary>Associates syntactic C# type arguments.</summary>
public sealed class SyntacticCSharpTypeAssociator
    : IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector>
{
    /// <summary>Instantiates a <see cref="SyntacticCSharpTypeAssociator"/>, associating syntactic C# type arguments.</summary>
    public SyntacticCSharpTypeAssociator() { }

    void IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingSyntacticCSharpTypeAssociationQueryResponseCollector>.Handle(
        IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData> query,
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
