using System.Threading.Tasks;
using Common.ResultOf;
using Core.Domain;
using Core.Domain.Common.Types;
using R3;

namespace UI.Machines;

public interface IMachine
{
    Subject<ResultOf<Logfile>> LogfileCreated { get; }

    BehaviorSubject<StateType> State { get; }

    void Start();
    void Stop();
    Task SendAcknowledgmentAsync(string serialNumber);
}