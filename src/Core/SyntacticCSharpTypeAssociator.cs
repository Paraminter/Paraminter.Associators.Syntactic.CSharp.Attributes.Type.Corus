namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Associators.Queries;
using Paraminter.CSharp.Type.Corus.Common;
using Paraminter.CSharp.Type.Corus.Queries;
using Paraminter.CSharp.Type.Queries.Handlers;
using Paraminter.Queries.Handlers;

using System;

/// <summary>Associates syntactic C# type arguments.</summary>
public sealed class SyntacticCSharpTypeAssociator
    : IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler>
{
    /// <summary>Instantiates a <see cref="SyntacticCSharpTypeAssociator"/>, associating syntactic C# type arguments.</summary>
    public SyntacticCSharpTypeAssociator() { }

    void IQueryHandler<IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData>, IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler>.Handle(
        IAssociateArgumentsQuery<IAssociateSyntacticCSharpTypeData> query,
        IInvalidatingAssociateSyntacticCSharpTypeQueryResponseHandler queryResponseHandler)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (queryResponseHandler is null)
        {
            throw new ArgumentNullException(nameof(queryResponseHandler));
        }

        if (query.Data.Parameters.Count != query.Data.SyntacticArguments.Count)
        {
            queryResponseHandler.Invalidator.Handle(InvalidateQueryResponseCommand.Instance);

            return;
        }

        for (var i = 0; i < query.Data.Parameters.Count; i++)
        {
            var parameter = query.Data.Parameters[i];
            var syntacticArgument = query.Data.SyntacticArguments[i];

            var command = new AddCSharpTypeAssociationCommand(parameter, syntacticArgument);

            queryResponseHandler.AssociationCollector.Handle(command);
        }
    }
}
