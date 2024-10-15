namespace Paraminter.Associating.CSharp.Type.Corus;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.CSharp.Type.Corus.Errors;
using Paraminter.Associating.CSharp.Type.Corus.Models;
using Paraminter.Cqs;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Type.Models;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<IAssociateCSharpTypeArgumentsData>> Sut { get; }

    public abstract Mock<ICommandHandler<IPairArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> PairerMock { get; }
    public abstract Mock<ICSharpTypeAssociatorErrorHandler> ErrorHandlerMock { get; }
}
