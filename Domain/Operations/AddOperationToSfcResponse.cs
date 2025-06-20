using Domain.Core.Types;

namespace Domain.Operations;

public record AddOperationToSfcResponse(
    UploadResultType UploadResult,
    FileInfo? UploadResponse = null);