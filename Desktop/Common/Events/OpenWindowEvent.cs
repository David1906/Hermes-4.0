using System;
using Desktop.Core;
using Paramore.Brighter;

namespace Desktop.Common.Events;

public class OpenWindowEvent() : Event(Guid.NewGuid())
{
    public required ViewModelBase ViewModel { get; set; }
    public bool IsCached { get; set; }
}