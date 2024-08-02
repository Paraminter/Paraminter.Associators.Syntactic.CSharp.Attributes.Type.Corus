namespace Paraminter.CSharp.Type.Corus;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.CSharp.Type.Commands;
using Paraminter.CSharp.Type.Corus.Commands;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IRecordCSharpTypeAssociationCommand>> recorderMock = new();

        SyntacticCSharpTypeAssociator sut = new(recorderMock.Object);

        return new Fixture(sut, recorderMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> Sut;

        private readonly Mock<ICommandHandler<IRecordCSharpTypeAssociationCommand>> RecorderMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> sut,
            Mock<ICommandHandler<IRecordCSharpTypeAssociationCommand>> recorderMock)
        {
            Sut = sut;

            RecorderMock = recorderMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IRecordCSharpTypeAssociationCommand>> IFixture.RecorderMock => RecorderMock;
    }
}
