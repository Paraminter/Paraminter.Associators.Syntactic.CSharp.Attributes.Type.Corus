namespace Paraminter.Associating.CSharp.Type.Corus.Errors;

using Moq;

using Paraminter.Associating.CSharp.Type.Corus.Errors.Commands;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        var differentNumberOfArgumentsAndParametersMock = new Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>>();

        var sut = new CSharpTypeAssociatorErrorHandler(differentNumberOfArgumentsAndParametersMock.Object);

        return new Fixture(sut, differentNumberOfArgumentsAndParametersMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICSharpTypeAssociatorErrorHandler Sut;

        private readonly Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> DifferentNumberOfArgumentsAndParametersMock;

        public Fixture(
            ICSharpTypeAssociatorErrorHandler sut,
            Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> differentNumberOfArgumentsAndParametersMock)
        {
            Sut = sut;

            DifferentNumberOfArgumentsAndParametersMock = differentNumberOfArgumentsAndParametersMock;
        }

        ICSharpTypeAssociatorErrorHandler IFixture.Sut => Sut;

        Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> IFixture.DifferentNumberOfArgumentsAndParametersMock => DifferentNumberOfArgumentsAndParametersMock;
    }
}
