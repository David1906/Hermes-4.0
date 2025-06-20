using Domain.Logfiles;

namespace UseCases.Logfiles;

public record MoveLogfileToBackupCommand(Logfile Logfile, DirectoryInfo DestinationDirectory);