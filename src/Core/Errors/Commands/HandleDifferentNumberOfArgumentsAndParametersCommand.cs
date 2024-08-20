namespace Paraminter.CSharp.Type.Corus.Errors.Commands;

internal sealed class HandleDifferentNumberOfArgumentsAndParametersCommand
    : IHandleDifferentNumberOfArgumentsAndParametersCommand
{
    public static IHandleDifferentNumberOfArgumentsAndParametersCommand Instance { get; } = new HandleDifferentNumberOfArgumentsAndParametersCommand();

    private HandleDifferentNumberOfArgumentsAndParametersCommand() { }
}
