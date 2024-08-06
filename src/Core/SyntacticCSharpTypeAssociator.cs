namespace Paraminter.CSharp.Type.Corus;

using Paraminter.Arguments.CSharp.Type.Models;
using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.CSharp.Type.Corus.Common;
using Paraminter.CSharp.Type.Corus.Models;
using Paraminter.Parameters.Type.Models;
using Paraminter.Semantic.Type.Apheleia.Common;

using System;

/// <summary>Associates syntactic C# type arguments.</summary>
public sealed class SyntacticCSharpTypeAssociator
    : ICommandHandler<IAssociateArgumentsCommand<IAssociateSyntacticCSharpTypeData>>
{
    private readonly ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>> Recorder;
    private readonly ICommandHandler<IInvalidateArgumentAssociationsRecordCommand> Invalidator;

    /// <summary>Instantiates a <see cref="SyntacticCSharpTypeAssociator"/>, associating syntactic C# type arguments.</summary>
    /// <param name="recorder">Records associated syntactic C# type arguments.</param>
    /// <param name="invalidator">Invalidates the record of associated syntactic C# type arguments.</param>
    public SyntacticCSharpTypeAssociator(
        ICommandHandler<IRecordArgumentAssociationCommand<ITypeParameter, ICSharpTypeArgumentData>> recorder,
        ICommandHandler<IInvalidateArgumentAssociationsRecordCommand> invalidator)
    {
        Recorder = recorder ?? throw new ArgumentNullException(nameof(recorder));
        Invalidator = invalidator ?? throw new ArgumentNullException(nameof(invalidator));
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
            Invalidator.Handle(InvalidateArgumentAssociationsRecordCommand.Instance);

            return;
        }

        for (var i = 0; i < command.Data.Parameters.Count; i++)
        {
            var parameter = new TypeParameter(command.Data.Parameters[i]);
            var argumentData = new CSharpTypeArgumentData(command.Data.SyntacticArguments[i]);

            var recordCommand = new RecordCSharpTypeAssociationCommand(parameter, argumentData);

            Recorder.Handle(recordCommand);
        }
    }
}
