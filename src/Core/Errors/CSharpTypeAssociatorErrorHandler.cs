namespace Paraminter.CSharp.Type.Corus.Errors;

using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Errors.Commands;

using System;

/// <inheritdoc cref="ICSharpTypeAssociatorErrorHandler"/>
public sealed class CSharpTypeAssociatorErrorHandler
    : ICSharpTypeAssociatorErrorHandler
{
    private readonly ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> DifferentNumberOfArgumentsAndParameters;

    /// <summary>Instantiates a handler of errors encountered when associating syntactic C# type arguments with parameters.</summary>
    /// <param name="differentNumberOfArgumentsAndParameters">Handles there being a different number of arguments and parameters.</param>
    public CSharpTypeAssociatorErrorHandler(
        ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> differentNumberOfArgumentsAndParameters)
    {
        DifferentNumberOfArgumentsAndParameters = differentNumberOfArgumentsAndParameters ?? throw new ArgumentNullException(nameof(differentNumberOfArgumentsAndParameters));
    }

    ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> ICSharpTypeAssociatorErrorHandler.DifferentNumberOfArgumentsAndParameters => DifferentNumberOfArgumentsAndParameters;
}
