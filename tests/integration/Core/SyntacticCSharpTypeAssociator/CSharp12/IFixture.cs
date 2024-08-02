namespace Paraminter.CSharp.Type.Corus;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.CSharp.Type.Commands;
using Paraminter.CSharp.Type.Corus.Commands;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> Sut { get; }

    public abstract Mock<ICommandHandler<IRecordCSharpTypeAssociationCommand>> RecorderMock { get; }
}
