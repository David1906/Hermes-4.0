using Domain.Logfiles;

namespace UseCases.Operations;

public record ProcessOperationCommand(
    Logfile InputLogfile,
    DirectoryInfo BackupDirectory,
    string OkResponses,
    TimeSpan Timeout,
    int MaxRetries = 0);