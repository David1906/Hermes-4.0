using Common;
using Common.ResultOf;
using Core.Application.Common.FileBuilders;
using Core.Domain;
using Core.Domain.Common.Types;

namespace Infrastructure.FileBuilders;

public class TriLogFileBuilder(IResilientFileSystem fileSystem) : ITriLogFileBuilder
{
    private readonly Random _random = new();
    private readonly List<Defect> _defects = [];
    private string _serialNumber = $"TEST{Guid.NewGuid()}";
    private bool _isPass = true;
    private DateTime _startTime = DateTime.Now;
    private DateTime _endTime = DateTime.Now;
    private string _stationId = "112233";
    private string _userId = "GKG";
    private string _productId = "ALL";

    public ITriLogFileBuilder Clone()
    {
        return new TriLogFileBuilder(fileSystem);
    }

    public ITriLogFileBuilder SerialNumber(string serialNumber)
    {
        this._serialNumber = serialNumber;
        return this;
    }

    public ITriLogFileBuilder IsPass(bool isPass)
    {
        this._isPass = isPass;
        return this;
    }

    public ITriLogFileBuilder StartTime(DateTime startTime)
    {
        this._startTime = startTime;
        return this;
    }

    public ITriLogFileBuilder EndTime(DateTime endTime)
    {
        this._endTime = endTime;
        return this;
    }

    public ITriLogFileBuilder StationId(string stationId)
    {
        this._stationId = stationId;
        return this;
    }

    public async Task<ResultOf<string>> WriteAsync(string destination, CancellationToken cancellationToken = default)
    {
        try
        {
            await fileSystem.WriteAllTextAsync(destination, BuildContent(), cancellationToken);
            return ResultOf<string>.Success(destination);
        }
        catch (Exception e)
        {
            return ResultOf<string>.Failure(e.Message);
        }
    }

    private string BuildContent()
    {
        var content = $"""
                       {_serialNumber}
                       {_stationId}
                       {_userId}
                       {_productId}
                       {(_isPass ? "PASS" : "FAIL")}
                       {_startTime:ddMMyyHHmmss}
                       {_endTime:ddMMyyHHmmss}
                       NA
                       0
                       0
                       Error flag,Recipe name,Paste ID,CAD link Gerber,Error code,Multi Number 
                       """;

        return _defects.Aggregate(content, (current, defect) =>
            current +
            $"\n{(defect.ErrorFlag.ToString().ToUpper())};ALL;NA;{defect.Location};{defect.ErrorCode};1");
    }

    public ITriLogFileBuilder UserId(string userId)
    {
        this._userId = userId;
        return this;
    }

    public ITriLogFileBuilder ProductId(string productId)
    {
        this._productId = productId;
        return this;
    }

    public ITriLogFileBuilder AddRandomDefect(string? location = null, bool isBad = false)
    {
        var rnd = this._random.Next();
        this._defects.Add(new Defect()
        {
            ErrorFlag = isBad ? ErrorFlagType.Bad : ErrorFlagType.Good,
            Location = location ?? $"L{rnd}",
            ErrorCode = $"EC{rnd}"
        });
        return this;
    }
}