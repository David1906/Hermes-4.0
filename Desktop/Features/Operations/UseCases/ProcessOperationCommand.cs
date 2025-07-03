using System;
using System.IO;
using Desktop.Core.Types;
using Desktop.Features.Logfiles.Domain;

namespace Desktop.Features.Operations.UseCases;

public record ProcessOperationCommand(
    Logfile InputLogfile,
    LogfileType LogfileType,
    DirectoryInfo BackupDirectory,
    string OkResponses,
    TimeSpan Timeout,
    int MaxRetries = 0);