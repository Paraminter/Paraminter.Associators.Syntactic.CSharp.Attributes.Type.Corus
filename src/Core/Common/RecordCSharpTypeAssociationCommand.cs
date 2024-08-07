namespace Paraminter.CSharp.Type.Corus.Common;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;

internal sealed class RecordCSharpTypeAssociationCommand
    : IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>
{
    private readonly ITypeParameter Parameter;
    private readonly ICSharpTypeArgumentData ArgumentData;

    public RecordCSharpTypeAssociationCommand(
        ITypeParameter parameter,
        ICSharpTypeArgumentData argumentData)
    {
        Parameter = parameter;
        ArgumentData = argumentData;
    }

    ITypeParameter IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>.Parameter => Parameter;
    ICSharpTypeArgumentData IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>.ArgumentData => ArgumentData;
}
