namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.CSharp.Type.Commands;
using Paraminter.CSharp.Type.Corus.Commands;
using Paraminter.CSharp.Type.Corus.Common;

using System;

/// <summary>Associates syntactic C# type arguments.</summary>
public sealed class SyntacticCSharpTypeAssociator
    : ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>>
{
    private readonly ICommandHandler<IRecordCSharpTypeAssociationCommand> Recorder;

    /// <summary>Instantiates a <see cref="SyntacticCSharpTypeAssociator"/>, associating syntactic C# type arguments.</summary>
    /// <param name="recorder">Records associated syntactic C# type arguments.</param>
    public SyntacticCSharpTypeAssociator(
        ICommandHandler<IRecordCSharpTypeAssociationCommand> recorder)
    {
        Recorder = recorder ?? throw new ArgumentNullException(nameof(recorder));
    }

    void ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>>.Handle(
        IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData> command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (command.Data.Parameters.Count != command.Data.SyntacticArguments.Count)
        {
            return;
        }

        for (var i = 0; i < command.Data.Parameters.Count; i++)
        {
            var parameter = command.Data.Parameters[i];
            var syntacticArgument = command.Data.SyntacticArguments[i];

            var recordCommand = new RecordCSharpTypeAssociationCommand(parameter, syntacticArgument);

            Recorder.Handle(recordCommand);
        }
    }
}
