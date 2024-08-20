namespace Paraminter.CSharp.Type.Corus;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Errors;
using Paraminter.CSharp.Type.Corus.Models;
using Paraminter.Parameters.Type.Models;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> individualAssociatorMock = new();
        Mock<ICSharpTypeAssociatorErrorHandler> errorHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        CSharpTypeAssociator sut = new(individualAssociatorMock.Object, errorHandlerMock.Object);

        return new Fixture(sut, individualAssociatorMock, errorHandlerMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSyntacticCSharpTypeArgumentsData>> Sut;

        private readonly Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> IndividualAssociatorMock;
        private readonly Mock<ICSharpTypeAssociatorErrorHandler> ErrorHandlerMock;

        public Fixture(
            ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSyntacticCSharpTypeArgumentsData>> sut,
            Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> individualAssociatorMock,
            Mock<ICSharpTypeAssociatorErrorHandler> errorHandlerMock)
        {
            Sut = sut;

            IndividualAssociatorMock = individualAssociatorMock;
            ErrorHandlerMock = errorHandlerMock;
        }

        ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSyntacticCSharpTypeArgumentsData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> IFixture.IndividualAssociatorMock => IndividualAssociatorMock;
        Mock<ICSharpTypeAssociatorErrorHandler> IFixture.ErrorHandlerMock => ErrorHandlerMock;
    }
}
