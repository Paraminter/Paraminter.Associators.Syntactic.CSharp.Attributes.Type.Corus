namespace Paraminter.Associating.CSharp.Type.Corus.Errors;

using Moq;

using Paraminter.Associating.CSharp.Type.Corus.Errors.Commands;
using Paraminter.Cqs;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullDifferentNumberOfArgumentsAndParameters_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsHandler()
    {
        var result = Target(Mock.Of<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>>());

        Assert.NotNull(result);
    }

    private static CSharpTypeAssociatorErrorHandler Target(
        ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand> differentNumberOfArgumentsAndParameters)
    {
        return new CSharpTypeAssociatorErrorHandler(differentNumberOfArgumentsAndParameters);
    }
}
