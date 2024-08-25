namespace Paraminter.CSharp.Type.Corus;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Commands;
using Paraminter.CSharp.Type.Corus.Errors;
using Paraminter.CSharp.Type.Corus.Errors.Commands;
using Paraminter.CSharp.Type.Corus.Models;
using Paraminter.Parameters.Type.Models;
using Paraminter.Semantic.Type.Apheleia.Models;

using System;

/// <summary>Associates C# type arguments with parameters.</summary>
public sealed class CSharpTypeAssociator
    : ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllCSharpTypeArgumentsData>>
{
    private readonly ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>> IndividualAssociator;
    private readonly ICSharpTypeAssociatorErrorHandler ErrorHandler;

    /// <summary>Instantiates an associator of C# type arguments with parameters.</summary>
    /// <param name="individualAssociator">Associates individual C# type arguments with parameters.</param>
    /// <param name="errorHandler">Handles encountered errors.</param>
    public CSharpTypeAssociator(
        ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>> individualAssociator,
        ICSharpTypeAssociatorErrorHandler errorHandler)
    {
        IndividualAssociator = individualAssociator ?? throw new ArgumentNullException(nameof(individualAssociator));
        ErrorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
    }

    void ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllCSharpTypeArgumentsData>>.Handle(
        IAssociateAllArgumentsCommand<IAssociateAllCSharpTypeArgumentsData> command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (command.Data.Parameters.Count != command.Data.SyntacticArguments.Count)
        {
            ErrorHandler.DifferentNumberOfArgumentsAndParameters.Handle(HandleDifferentNumberOfArgumentsAndParametersCommand.Instance);

            return;
        }

        for (var i = 0; i < command.Data.Parameters.Count; i++)
        {
            AssociateArgument(command.Data.Parameters[i], command.Data.SyntacticArguments[i]);
        }
    }

    private void AssociateArgument(
        ITypeParameterSymbol parameterSymbol,
        TypeSyntax syntacticArgument)
    {
        var parameter = new TypeParameter(parameterSymbol);
        var argumentData = new CSharpTypeArgumentData(syntacticArgument);

        var command = new AssociateSingleArgumentCommand(parameter, argumentData);

        IndividualAssociator.Handle(command);
    }
}
