using Domain.Logfiles;

namespace UseCases.Logfiles;

public record AddLogfileToSfcCommand(
    Logfile LogfileToUpload,
    string OkResponses,
    TimeSpan Timeout,
    DirectoryInfo BackupDirectory,
    int MaxRetries = 0);