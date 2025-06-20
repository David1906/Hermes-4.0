using Domain.Core.Errors;
using Domain.Core.Types;
using Domain.Logfiles;
using OneOf;
using R3;

namespace Domain.Machines;

public interface IMachine
{
    Subject<OneOf<Logfile, Error>> LogfileCreated { get; }

    BehaviorSubject<StateType> State { get; }

    void Start();
    void Stop();
}