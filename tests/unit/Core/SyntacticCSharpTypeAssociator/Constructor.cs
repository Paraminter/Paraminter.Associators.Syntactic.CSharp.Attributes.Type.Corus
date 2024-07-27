namespace Paraminter.CSharp.Type.Corus;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void ReturnsAssociator()
    {
        var result = Target();

        Assert.NotNull(result);
    }

    private static SyntacticCSharpTypeAssociator Target() => new();
}
