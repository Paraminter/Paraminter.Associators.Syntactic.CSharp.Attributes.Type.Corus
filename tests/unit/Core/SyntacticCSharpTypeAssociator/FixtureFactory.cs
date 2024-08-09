namespace Paraminter.CSharp.Type.Corus;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Models;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>> recorderMock = new();
        Mock<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>> invalidatorMock = new();

        SyntacticCSharpTypeAssociator sut = new(recorderMock.Object, invalidatorMock.Object);

        return new Fixture(sut, recorderMock, invalidatorMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> Sut;

        private readonly Mock<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>> RecorderMock;
        private readonly Mock<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>> InvalidatorMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> sut,
            Mock<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>> recorderMock,
            Mock<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>> invalidatorMock)
        {
            Sut = sut;

            RecorderMock = recorderMock;
            InvalidatorMock = invalidatorMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>> IFixture.RecorderMock => RecorderMock;
        Mock<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>> IFixture.InvalidatorMock => InvalidatorMock;
    }
}
