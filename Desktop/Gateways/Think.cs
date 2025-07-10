using System.Threading.Tasks;
using Core.Domain;
using Panel = Desktop.Features.Panels.Domain.Panel;

namespace Desktop.Gateways;

public class Think
{
    public async Task<Operation> DoSomethingWithPanel(Panel panel)
    {
        var operation = new Operation { Type = OperationType.SendPanelToNextStation };
        operation.Start();
        // Dot things
        operation.Result = OperationResultType.Pass;
        operation.End();
        return await Task.FromResult(operation);
    }
}