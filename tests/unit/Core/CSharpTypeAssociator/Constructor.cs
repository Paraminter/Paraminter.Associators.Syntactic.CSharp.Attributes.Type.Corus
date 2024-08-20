namespace Paraminter.CSharp.Type.Corus;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Errors;
using Paraminter.Parameters.Type.Models;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullIndividualAssociator_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<ICSharpTypeAssociatorErrorHandler>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullErrorHandler_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>>(), Mock.Of<ICSharpTypeAssociatorErrorHandler>());

        Assert.NotNull(result);
    }

    private static CSharpTypeAssociator Target(
        ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>> individualAssociator,
        ICSharpTypeAssociatorErrorHandler errorHandler)
    {
        return new CSharpTypeAssociator(individualAssociator, errorHandler);
    }
}
