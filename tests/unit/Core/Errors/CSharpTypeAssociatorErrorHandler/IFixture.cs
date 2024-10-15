namespace Paraminter.Associating.CSharp.Type.Corus.Errors;

using Moq;

using Paraminter.Associating.CSharp.Type.Corus.Errors.Commands;
using Paraminter.Cqs;

internal interface IFixture
{
    public abstract ICSharpTypeAssociatorErrorHandler Sut { get; }

    public abstract Mock<ICommandHandler<IHandleDifferentNumberOfArgumentsAndParametersCommand>> DifferentNumberOfArgumentsAndParametersMock { get; }
}
