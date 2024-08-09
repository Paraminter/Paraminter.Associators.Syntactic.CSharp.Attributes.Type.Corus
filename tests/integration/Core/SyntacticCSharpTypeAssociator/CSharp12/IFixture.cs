namespace Paraminter.CSharp.Type.Corus;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Models;
using Paraminter.Parameters.Type.Models;
using Paraminter.Recorders.Commands;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>> Sut { get; }

    public abstract Mock<ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>>> RecorderMock { get; }
    public abstract Mock<ICommandHandler<IInvalidateArgumentAssociationsRecordCommand>> InvalidatorMock { get; }
}
