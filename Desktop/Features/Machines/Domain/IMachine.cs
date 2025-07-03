using Common.ResultOf;
using Desktop.Core.Types;
using Desktop.Features.Logfiles.Domain;
using R3;
using System.Threading.Tasks;

namespace Desktop.Features.Machines.Domain;

public interface IMachine
{
    Subject<ResultOf<Logfile>> LogfileCreated { get; }

    BehaviorSubject<StateType> State { get; }

    void Start();
    void Stop();
    Task SendAcknowledgmentAsync(string serialNumber);
}