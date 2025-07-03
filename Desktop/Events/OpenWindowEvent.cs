using Desktop.Core;
using Paramore.Brighter;
using System;

namespace Desktop.Events;

public class OpenWindowEvent() : Event(Guid.NewGuid())
{
    public required ViewModelBase ViewModel { get; set; }
}