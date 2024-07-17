namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Collectors;
using Paraminter.Queries.Handlers;

using System;

/// <summary>Associates syntactic data about C# type arguments and C# type parameters.</summary>
public sealed class SyntacticCSharpTypeInvocationDataAssociator
    : IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, ISyntacticCSharpTypeInvocationDataAssociatorQueryResponseCollector>
{
    /// <summary>Instantiates a <see cref="SyntacticCSharpTypeInvocationDataAssociator"/>, associating syntactic data about C# type arguments and C# type parameters.</summary>
    public SyntacticCSharpTypeInvocationDataAssociator() { }

    void IQueryHandler<IGetAssociatedInvocationDataQuery<IUnassociatedSyntacticCSharpTypeInvocationData>, ISyntacticCSharpTypeInvocationDataAssociatorQueryResponseCollector>.Handle(
        IGetAssociatedInvocationDataQuery<IUnassociatedSyntacticCSharpTypeInvocationData> query,
        ISyntacticCSharpTypeInvocationDataAssociatorQueryResponseCollector queryResponseCollector)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (queryResponseCollector is null)
        {
            throw new ArgumentNullException(nameof(queryResponseCollector));
        }

        if (query.UnassociatedInvocationData.Parameters.Count != query.UnassociatedInvocationData.SyntacticArguments.Count)
        {
            queryResponseCollector.Invalidator.Invalidate();

            return;
        }

        for (var i = 0; i < query.UnassociatedInvocationData.Parameters.Count; i++)
        {
            var parameter = query.UnassociatedInvocationData.Parameters[i];
            var argumentData = query.UnassociatedInvocationData.SyntacticArguments[i];

            queryResponseCollector.Associations.Add(parameter, argumentData);
        }
    }
}
