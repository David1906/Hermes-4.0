using Core.Application.Common.Types;
using Paramore.Brighter;

namespace Core.Application.Common.Events;

public class ShowStopEvent() : Event(Guid.NewGuid())
{
    public string Title { get; set; } = "OK";
    public string SerialNumber { get; set; } = "";
    public List<DepartmentType> Departments { get; init; } = [];
}