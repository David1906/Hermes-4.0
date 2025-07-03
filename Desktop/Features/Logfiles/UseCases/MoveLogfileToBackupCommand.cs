using System.IO;
using Desktop.Features.Logfiles.Domain;

namespace Desktop.Features.Logfiles.UseCases;

public record MoveLogfileToBackupCommand(Logfile Logfile, DirectoryInfo DestinationDirectory);