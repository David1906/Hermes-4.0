using Common;
using Domain.Operations;
using ROP;

namespace Data.Sfc;

public interface ISfcApi
{
    Task<Result<TextDocument>> UploadOperationAsync(
        AddOperationToSfcRequest requestOperationDto,
        CancellationToken ct = default);
}