namespace Paraminter.CSharp.Type.Corus.Common;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Paraminter.CSharp.Type.Commands;

internal sealed class AddCSharpTypeAssociationCommand
    : IAddCSharpTypeAssociationCommand
{
    private readonly ITypeParameterSymbol Parameter;
    private readonly TypeSyntax SyntacticArgument;

    public AddCSharpTypeAssociationCommand(
        ITypeParameterSymbol parameter,
        TypeSyntax syntacticArgument)
    {
        Parameter = parameter;
        SyntacticArgument = syntacticArgument;
    }

    ITypeParameterSymbol IAddCSharpTypeAssociationCommand.Parameter => Parameter;
    TypeSyntax IAddCSharpTypeAssociationCommand.SyntacticArgument => SyntacticArgument;
}
