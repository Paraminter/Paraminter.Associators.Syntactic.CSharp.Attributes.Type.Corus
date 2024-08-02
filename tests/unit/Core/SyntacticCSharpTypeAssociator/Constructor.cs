namespace Paraminter.CSharp.Type.Corus;

using Moq;

using Paraminter.Commands.Handlers;
using Paraminter.CSharp.Type.Commands;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullRecorder_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IRecordCSharpTypeAssociationCommand>>());

        Assert.NotNull(result);
    }

    private static SyntacticCSharpTypeAssociator Target(
        ICommandHandler<IRecordCSharpTypeAssociationCommand> recorder)
    {
        return new SyntacticCSharpTypeAssociator(recorder);
    }
}
