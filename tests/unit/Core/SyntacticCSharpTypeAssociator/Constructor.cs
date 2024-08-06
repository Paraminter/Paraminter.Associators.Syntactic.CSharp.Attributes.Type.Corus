namespace Paraminter.CSharp.Type.Corus;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Parameters.Type.Models;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullRecorder_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullInvalidator_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>>(), Mock.Of<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>>());

        Assert.NotNull(result);
    }

    private static SyntacticCSharpTypeAssociator Target(
        ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>> recorder,
        ICommandHandler<IInvalidateArgumentAssociationsRecordCommand> invalidator)
    {
        return new SyntacticCSharpTypeAssociator(recorder, invalidator);
    }
}
