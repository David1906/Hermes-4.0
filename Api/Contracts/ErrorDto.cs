namespace Api.Contracts;

public class ErrorDto
{
    public string Message { get; set; } = "";

    public Guid? ErrorCode { get; set; }
}