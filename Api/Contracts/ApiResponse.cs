namespace Api.Contracts;

public record ApiResponse<T>
{
    public T? Data { get; set; }
    public bool Status { get; set; }
    public IEnumerable<ErrorDto> Errors { get; set; } = [];

    public static ApiResponse<T> Success(T data)
    {
        return new ApiResponse<T>
        {
            Status = true,
            Data = data
        };
    }

    public static ApiResponse<T> Failed(IEnumerable<ErrorDto> errors)
    {
        return new ApiResponse<T>
        {
            Status = false,
            Errors = errors
        };
    }
}