namespace Paraminter.CSharp.Type.Corus.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Paraminter.Models;

using System.Collections.Generic;

/// <summary>Represents data used to associate all syntactic C# type arguments with parameters.</summary>
public interface IAssociateAllSyntacticCSharpTypeArgumentsData
    : IAssociateAllArgumentsData
{
    /// <summary>The type parameters.</summary>
    public abstract IReadOnlyList<ITypeParameterSymbol> Parameters { get; }

    /// <summary>The syntactic C# type arguments.</summary>
    public abstract IReadOnlyList<TypeSyntax> SyntacticArguments { get; }
}
