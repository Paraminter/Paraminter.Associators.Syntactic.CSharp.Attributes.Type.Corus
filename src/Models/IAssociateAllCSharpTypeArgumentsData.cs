namespace Paraminter.Associating.CSharp.Type.Corus.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Paraminter.Associating.Models;

using System.Collections.Generic;

/// <summary>Represents data used to associate syntactic C# type arguments with parameters.</summary>
public interface IAssociateCSharpTypeArgumentsData
    : IAssociateArgumentsData
{
    /// <summary>The type parameters.</summary>
    public abstract IReadOnlyList<ITypeParameterSymbol> Parameters { get; }

    /// <summary>The syntactic C# type arguments.</summary>
    public abstract IReadOnlyList<TypeSyntax> SyntacticArguments { get; }
}
