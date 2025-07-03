using Common.ResultOf;
using Common;
using Desktop.Core;
using Desktop.Features.Locking;
using Domain.Logfiles;
using Domain.OperationTasks;
using Domain.Operations;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace Desktop.Features;

public class ViewModelFactory(
    IServiceProvider serviceProvider,
    IOperationsRepository operationsRepository,
    IResilientFileSystem resilientFileSystem
)
{
    public async Task<ResultOf<ViewModelBase>> CreateStopAsync(Operation operation)
    {
        var stopViewModel = serviceProvider.GetRequiredService<StopViewModel>();
        stopViewModel.Title = await ReadFileAsync(operation.Logfile);
        stopViewModel.SerialNumber = operation.MainSerialNumber;
        return stopViewModel;
    }

    private async Task<string> ReadFileAsync(Logfile? logfile)
    {
        return logfile is null
            ? ""
            : await resilientFileSystem.ReadAllTextAsync(logfile.FullName);
    }
}