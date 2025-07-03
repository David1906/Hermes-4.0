using System;
using System.IO;
using Desktop.Features.Logfiles.Domain;

namespace Desktop.Features.Logfiles.UseCases;

public record AddLogfileToSfcCommand(
    Logfile LogfileToUpload,
    string OkResponses,
    TimeSpan Timeout,
    DirectoryInfo BackupDirectory,
    int MaxRetries = 0);