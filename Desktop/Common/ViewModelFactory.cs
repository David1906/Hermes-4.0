using Common.ResultOf;
using Common;
using Desktop.Core.Types;
using Desktop.Core;
using Desktop.Features.Logfiles.Domain;
using Desktop.Features.OperationTasks.Domain;
using Desktop.Features.Stops.Delivery;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using Desktop.Features.Operations.Delivery;

namespace Desktop.Common;

public class ViewModelFactory(
    IServiceProvider serviceProvider,
    IResilientFileSystem resilientFileSystem
)
{
    public async Task<ResultOf<ViewModelBase>> CreateStopAsync(OperationTask operationTask)
    {
        var stopViewModel = serviceProvider.GetRequiredService<StopViewModel>();
        stopViewModel.Title = operationTask.IsFailure
            ? operationTask.Result == OperationTaskResultType.TimedOut
                ? "Operation timed out, please contact IT support."
                : operationTask.Result.ToString()
            : await this.ReadFileAsync(operationTask.Logfile);
        await ReadFileAsync(operationTask.Logfile); // TODO: implement Stops
        stopViewModel.SerialNumber = operationTask.Type.ToString();
        return stopViewModel;
    }

    private async Task<string> ReadFileAsync(Logfile? logfile)
    {
        return logfile is null
            ? ""
            : await resilientFileSystem.ReadAllTextAsync(logfile.FullName);
    }

    public Task<ResultOf<ViewModelBase>> CreateSuccess(string mainSerialNumber)
    {
        var successViewModel = serviceProvider.GetRequiredService<SuccessViewModel>();
        successViewModel.SerialNumber = mainSerialNumber;
        return Task.FromResult<ResultOf<ViewModelBase>>(successViewModel);
    }
}