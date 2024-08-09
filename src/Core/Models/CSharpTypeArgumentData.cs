namespace Paraminter.CSharp.Type.Corus.Models;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Paraminter.Arguments.CSharp.Type.Models;

internal sealed class CSharpTypeArgumentData
    : ICSharpTypeArgumentData
{
    private readonly TypeSyntax SyntacticArgument;

    public CSharpTypeArgumentData(
        TypeSyntax syntacticArgument)
    {
        SyntacticArgument = syntacticArgument;
    }

    TypeSyntax ICSharpTypeArgumentData.SyntacticArgument => SyntacticArgument;
}
