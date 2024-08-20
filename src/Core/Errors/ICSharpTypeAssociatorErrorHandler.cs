namespace Paraminter.CSharp.Type.Corus.Errors;

using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Errors.Commands;

/// <summary>Handles errors encountered when associating C# type arguments with parameters.</summary>
public interface ICSharpTypeAssociatorErrorHandler
{
    /// <summary>Handles there being a different number of arguments and parameters.</summary>
    public abstract ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> DifferentNumberOfArgumentsAndParameters { get; }
}
