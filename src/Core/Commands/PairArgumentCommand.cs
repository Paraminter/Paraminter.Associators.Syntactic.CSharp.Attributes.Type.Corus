namespace Paraminter.Associating.CSharp.Type.Corus.Commands;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

internal sealed class PairArgumentCommand
    : IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>
{
    private readonly ITypeParameter Parameter;
    private readonly ICSharpTypeArgumentData ArgumentData;

    public PairArgumentCommand(
        ITypeParameter parameter,
        ICSharpTypeArgumentData argumentData)
    {
        Parameter = parameter;
        ArgumentData = argumentData;
    }

    ITypeParameter IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>.Parameter => Parameter;
    ICSharpTypeArgumentData IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>.ArgumentData => ArgumentData;
}
