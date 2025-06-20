using Domain.Core.Types;
using Domain.Operations;
using R3;

namespace Data.Machines;

public abstract class MachineService
{
    public ReactiveProperty<Operation?> LogfileCreated { get; } = new();
    public ReactiveProperty<StateType> State { get; } = new(StateType.Stopped);

    public abstract void Start();
    public abstract void Stop();
}