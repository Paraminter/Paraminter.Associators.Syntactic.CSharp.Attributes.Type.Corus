﻿namespace Paraminter.Associating.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.CSharp.Type.Corus.Commands;
using Paraminter.Associating.CSharp.Type.Corus.Errors;
using Paraminter.Associating.CSharp.Type.Corus.Errors.Commands;
using Paraminter.Associating.CSharp.Type.Corus.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>Associates syntactic C# type arguments with parameters.</summary>
public sealed class CSharpTypeAssociator
    : ICommandHandler<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>>
{
    private readonly ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>> Pairer;
    private readonly ICSharpTypeAssociatorErrorHandler ErrorHandler;

    /// <summary>Instantiates an associator of syntactic C# type arguments with parameters.</summary>
    /// <param name="pairer">Pairs syntactic C# type arguments with parameters.</param>
    /// <param name="errorHandler">Handles encountered errors.</param>
    public CSharpTypeAssociator(
        ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>> pairer,
        ICSharpTypeAssociatorErrorHandler errorHandler)
    {
        Pairer = pairer ?? throw new ArgumentNullException(nameof(pairer));
        ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
    }

    async Task ICommandHandler<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>>.Handle(
        IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData> command,
        CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (command.Data.Parameters.Count != command.Data.SyntacticArguments.Count)
        {
            await ErrorHandler.DifferentNumberOfArgumentsAndParameters.Handle(HandleDifferentNumberOfArgumentsAndParametersCommand.Instance, cancellationToken).ConfigureAwait(false);

            return;
        }

        for (var i = 0; i < command.Data.Parameters.Count; i++)
        {
            await PairArgument(command.Data.Parameters[i], command.Data.SyntacticArguments[i], cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task PairArgument(
        ITypeParameterSymbol parameterSymbol,
        TypeSyntax syntacticArgument,
        CancellationToken cancellationToken)
    {
        var parameter = new TypeParameter(parameterSymbol);
        var argumentData = new CSharpTypeArgumentData(syntacticArgument);

        var command = new PairArgumentCommand(parameter, argumentData);

        await Pairer.Handle(command, cancellationToken).ConfigureAwait(false);
    }
}
