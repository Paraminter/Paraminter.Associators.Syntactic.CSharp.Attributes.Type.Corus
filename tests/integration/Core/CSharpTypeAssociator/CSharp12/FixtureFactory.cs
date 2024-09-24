namespace Paraminter.Associating.CSharp.Type.Corus;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.CSharp.Type.Corus.Errors;
using Paraminter.Associating.CSharp.Type.Corus.Models;
using Paraminter.Cqs.Handlers;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> pairerMock = new();
        Mock<ICSharpTypeAssociatorErrorHandler> errorHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        CSharpTypeAssociator sut = new(pairerMock.Object, errorHandlerMock.Object);

        return new Fixture(sut, pairerMock, errorHandlerMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>> Sut;

        private readonly Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> PairerMock;
        private readonly Mock<ICSharpTypeAssociatorErrorHandler> ErrorHandlerMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>> sut,
            Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> pairerMock,
            Mock<ICSharpTypeAssociatorErrorHandler> errorHandlerMock)
        {
            Sut = sut;

            PairerMock = pairerMock;
            ErrorHandlerMock = errorHandlerMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> IFixture.PairerMock => PairerMock;
        Mock<ICSharpTypeAssociatorErrorHandler> IFixture.ErrorHandlerMock => ErrorHandlerMock;
    }
}
