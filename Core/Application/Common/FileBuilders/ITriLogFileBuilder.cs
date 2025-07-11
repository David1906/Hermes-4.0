using Common.ResultOf;

namespace Core.Application.Common.FileBuilders;

public interface ITriLogFileBuilder
{
    ITriLogFileBuilder Clone();
    ITriLogFileBuilder SerialNumber(string serialNumber);
    ITriLogFileBuilder IsPass(bool isPass);
    ITriLogFileBuilder StartTime(DateTime startTime);
    ITriLogFileBuilder EndTime(DateTime endTime);
    ITriLogFileBuilder StationId(string stationId);
    Task<ResultOf<string>> WriteAsync(string destination, CancellationToken cancellationToken = default);
    ITriLogFileBuilder UserId(string userId);
    ITriLogFileBuilder ProductId(string productId);
    ITriLogFileBuilder AddRandomDefect(string? location = null, bool isBad = false);
}