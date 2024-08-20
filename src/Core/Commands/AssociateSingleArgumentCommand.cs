namespace Paraminter.CSharp.Type.Corus.Commands;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Parameters.Type.Models;

internal sealed class AssociateSingleArgumentCommand
    : IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>
{
    private readonly ITypeParameter Parameter;
    private readonly ICSharpTypeArgumentData ArgumentData;

    public AssociateSingleArgumentCommand(
        ITypeParameter parameter,
        ICSharpTypeArgumentData argumentData)
    {
        Parameter = parameter;
        ArgumentData = argumentData;
    }

    ITypeParameter IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>.Parameter => Parameter;
    ICSharpTypeArgumentData IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>.ArgumentData => ArgumentData;
}
