using Common.ResultOf;
using Domain.Core.Types;
using Domain.Logfiles;
using R3;

namespace Domain.Machines;

public interface IMachine
{
    Subject<ResultOf<Logfile>> LogfileCreated { get; }

    BehaviorSubject<StateType> State { get; }

    void Start();
    void Stop();
}