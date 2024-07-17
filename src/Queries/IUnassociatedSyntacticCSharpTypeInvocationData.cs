namespace Paraminter.CSharp.Type.Corus.Queries;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;

/// <summary>Represents unassociated syntactic C# type arguments of an invocation.</summary>
public interface IUnassociatedSyntacticCSharpTypeInvocationData
{
    /// <summary>The type parameters of the invocation.</summary>
    public abstract IReadOnlyList<ITypeParameterSymbol> Parameters { get; }

    /// <summary>The syntactic descriptions of the type arguments of the invocation.</summary>
    public abstract IReadOnlyList<TypeSyntax> SyntacticArguments { get; }
}
