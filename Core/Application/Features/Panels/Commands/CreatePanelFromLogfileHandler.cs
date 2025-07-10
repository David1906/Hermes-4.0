using Common.ResultOf;
using Core.Application.Common.Data;
using Core.Application.Common.FileParsers;
using Core.Domain;
using Paramore.Brighter;

namespace Core.Application.Features.Panels.Commands;

public class CreatePanelFromLogfileHandler(
    IUnitOfWork uow,
    IPanelParser panelParser,
    IOperationParser operationParser,
    IAmACommandProcessor commandProcessor
) : RequestHandlerAsync<CreatePanelFromLogfileCommand>
{
    public override async Task<CreatePanelFromLogfileCommand> HandleAsync(
        CreatePanelFromLogfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var panelResult = await panelParser
            .ParseAsync(command.Logfile, cancellationToken)
            .OnSuccess(panel => uow.PanelsRepository.AddAsync(panel, cancellationToken));

        if (panelResult.IsSuccess)
        {
            await operationParser.ParseMachineOperationAsync(command.Logfile)
                .OnSuccess(operation => panelResult.SuccessValue.Operations.Add(operation));
        }

        command.Result = panelResult;

        // TODO: Donde acomodar los eventos?
        // await commandProcessor.PublishAsync(
        //     new OperationCreatedEvent(command.Panel.MainSerialNumber),
        //     cancellationToken: cancellationToken);

        // foreach (var operationTask in result.SuccessValue.Operations)
        // {
        //     await commandProcessor.PublishAsync(new OperationTaskCreatedEvent(
        //         command.Operation.MainSerialNumber, operationTask), cancellationToken: cancellationToken);
        // }

        await uow.SaveChangesAsync(cancellationToken);
        return await base.HandleAsync(command, cancellationToken);
    }
}