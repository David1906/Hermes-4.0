using Domain.Core.Types;
using Domain.Defects;

namespace Domain.Builders;

public class TriLogfileBuilder
{
    private readonly Random _random = new();
    public readonly List<Defect> Defects = [];
    private string _serialNumber = $"TEST{Guid.NewGuid()}";
    private bool _isPass = true;
    private DateTime _startTime = DateTime.Now;
    private DateTime _endTime = DateTime.Now;
    private string _stationId = "123";
    private string _userId = "GKG";
    private string _productId = "ALL";

    public string Build()
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

        return Defects.Aggregate(content, (current, defect) =>
            current +
            $"\n{(defect.ErrorFlag.ToString().ToUpper())};ALL;NA;{defect.Location};{defect.ErrorCode};1");
    }

    public TriLogfileBuilder SerialNumber(string serialNumber)
    {
        this._serialNumber = serialNumber;
        return this;
    }

    public TriLogfileBuilder IsPass(bool isPass)
    {
        this._isPass = isPass;
        return this;
    }

    public TriLogfileBuilder StartTime(DateTime startTime)
    {
        this._startTime = startTime;
        return this;
    }

    public TriLogfileBuilder EndTime(DateTime endTime)
    {
        this._endTime = endTime;
        return this;
    }

    public TriLogfileBuilder StationId(string stationId)
    {
        this._stationId = stationId;
        return this;
    }

    public TriLogfileBuilder UserId(string userId)
    {
        this._userId = userId;
        return this;
    }

    public TriLogfileBuilder ProductId(string productId)
    {
        this._productId = productId;
        return this;
    }

    public TriLogfileBuilder AddRandomDefect(string? location = null, bool isBad = false)
    {
        var rnd = this._random.Next();
        this.Defects.Add(new Defect()
        {
            ErrorFlag = isBad ? ErrorFlagType.Bad : ErrorFlagType.Good,
            Location = location ?? $"L{rnd}",
            ErrorCode = $"EC{rnd}"
        });
        return this;
    }
}