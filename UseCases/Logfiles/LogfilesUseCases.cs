namespace UseCases.Logfiles;

public record LogfilesUseCases(
    AddLogfileToSfc AddLogfileToSfc,
    MoveLogfileToBackup MoveLogfileToBackup);