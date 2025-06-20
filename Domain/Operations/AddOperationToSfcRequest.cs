namespace Domain.Operations;

public record AddOperationToSfcRequest(
    FileInfo FileToUpload,
    string OkResponses,
    TimeSpan Timeout,
    int MaxRetries = 0);