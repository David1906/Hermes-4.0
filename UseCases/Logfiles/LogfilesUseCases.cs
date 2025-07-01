namespace UseCases.Logfiles;

public record LogfilesUseCases(
    AddLogfileToSfcUseCase AddLogfileToSfcUseCase,
    MoveLogfileToBackup MoveLogfileToBackup);