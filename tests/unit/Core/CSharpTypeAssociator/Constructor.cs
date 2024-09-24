namespace Paraminter.Associating.CSharp.Type.Corus;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Associating.CSharp.Type.Corus.Errors;
using Paraminter.Cqs.Handlers;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullPairer_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<ICSharpTypeAssociatorErrorHandler>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullErrorHandler_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>>(), Mock.Of<ICSharpTypeAssociatorErrorHandler>());

        Assert.NotNull(result);
    }

    private static CSharpTypeAssociator Target(
        ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>> pairer,
        ICSharpTypeAssociatorErrorHandler errorHandler)
    {
        return new CSharpTypeAssociator(pairer, errorHandler);
    }
}
