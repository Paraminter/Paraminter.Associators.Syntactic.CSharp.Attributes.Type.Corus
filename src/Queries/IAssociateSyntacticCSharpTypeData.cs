namespace Paraminter.CSharp.Type.Corus.Queries;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;

/// <summary>Represents data used to associate syntactic C# type arguments.</summary>
public interface IAssociateSyntacticCSharpTypeData
{
    /// <summary>The type parameters.</summary>
    public abstract IReadOnlyList<ITypeParameterSymbol> Parameters { get; }

    /// <summary>The syntactic C# type arguments.</summary>
    public abstract IReadOnlyList<TypeSyntax> SyntacticArguments { get; }
}
