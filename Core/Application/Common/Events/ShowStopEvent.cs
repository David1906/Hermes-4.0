using Core.Domain.Common.Types;
using Paramore.Brighter;

namespace Core.Application.Common.Events;

public class ShowStopEvent() : Event(Guid.NewGuid())
{
    public string Title { get; set; } = "OK";
    public string ErrorType { get; set; } = "";
    public string SerialNumber { get; set; } = "";
    public List<DepartmentType> Departments { get; init; } = [];
}