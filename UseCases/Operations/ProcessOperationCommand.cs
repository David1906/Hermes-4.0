using Domain.Core.Types;
using Domain.Logfiles;

namespace UseCases.Operations;

public record ProcessOperationCommand(
    Logfile InputLogfile,
    LogfileType LogfileType,
    DirectoryInfo BackupDirectory,
    string OkResponses,
    TimeSpan Timeout,
    int MaxRetries = 0);