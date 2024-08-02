namespace Paraminter.CSharp.Type.Corus.Common;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Paraminter.CSharp.Type.Commands;

internal sealed class RecordCSharpTypeAssociationCommand
    : IRecordCSharpTypeAssociationCommand
{
    private readonly ITypeParameterSymbol Parameter;
    private readonly TypeSyntax SyntacticArgument;

    public RecordCSharpTypeAssociationCommand(
        ITypeParameterSymbol parameter,
        TypeSyntax syntacticArgument)
    {
        Parameter = parameter;
        SyntacticArgument = syntacticArgument;
    }

    ITypeParameterSymbol IRecordCSharpTypeAssociationCommand.Parameter => Parameter;
    TypeSyntax IRecordCSharpTypeAssociationCommand.SyntacticArgument => SyntacticArgument;
}
