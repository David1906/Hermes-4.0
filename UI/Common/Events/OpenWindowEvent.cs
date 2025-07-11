using System;
using Paramore.Brighter;

namespace UI.Common.Events;

public class OpenWindowEvent() : Event(Guid.NewGuid())
{
    public required ViewModelBase ViewModel { get; set; }
    public bool IsCached { get; set; }
}