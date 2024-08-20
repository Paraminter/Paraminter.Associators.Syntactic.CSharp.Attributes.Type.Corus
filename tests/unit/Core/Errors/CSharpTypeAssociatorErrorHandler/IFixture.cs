namespace Paraminter.CSharp.Type.Corus.Errors;

using Moq;

using Paraminter.Cqs.Handlers;
using Paraminter.CSharp.Type.Corus.Errors.Commands;

internal interface IFixture
{
    public abstract ICSharpTypeAssociatorErrorHandler Sut { get; }

    public abstract Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> DifferentNumberOfArgumentsAndParametersMock { get; }
}
