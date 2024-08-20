namespace Paraminter.CSharp.Type.Corus;

using Moq;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Errors;
using Paraminter.CSharp.Type.Corus.Models;
using Paraminter.Parameters.Type.Models;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSyntacticCSharpTypeArgumentsData>> Sut { get; }

    public abstract Mock<ICommandHandler<IAssociateSingleArgumentCommand<ITypeParameter, ICSharpTypeArgumentData>>> IndividualAssociatorMock { get; }
    public abstract Mock<ICSharpTypeAssociatorErrorHandler> ErrorHandlerMock { get; }
}
