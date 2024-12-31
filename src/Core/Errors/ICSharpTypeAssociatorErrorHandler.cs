namespace Paraminter.Associating.CSharp.Type.Corus.Errors;

using Paraminter.Associating.CSharp.Type.Corus.Errors.Commands;

/// <summary>Handles errors encountered when associating syntactic C# type arguments with parameters.</summary>
public interface ICSharpTypeAssociatorErrorHandler
{
    /// <summary>Handles there being a different number of arguments and parameters.</summary>
    public abstract ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> DifferentNumberOfArgumentsAndParameters { get; }
}
