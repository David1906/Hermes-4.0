using Api.Contracts;
using ROP;
using System.Collections.Immutable;
using System.Net;

namespace Api.Extensions;

public static class RopExtension
{
    public static async Task<IResult> ToHttpResult<T>(this Task<Result<T>> resultTask)
    {
        var response = await resultTask;
        return response.Success
            ? Results.Ok(ApiResponse<T>.Success(response.Value))
            : response.HttpStatusCode switch
            {
                HttpStatusCode.BadRequest => Results.BadRequest(response.ToFailedApiResponse()),
                HttpStatusCode.Conflict => Results.Conflict(response.ToFailedApiResponse()),
                HttpStatusCode.Forbidden => Results.Forbid(),
                HttpStatusCode.NotFound => Results.NotFound(response.ToFailedApiResponse()),
                HttpStatusCode.Unauthorized => Results.Unauthorized(),
                _ => Results.InternalServerError(response.ToFailedApiResponse())
            };
    }

    private static ApiResponse<T> ToFailedApiResponse<T>(this Result<T> result)
        => ApiResponse<T>.Failed(result.Errors.ToErrorDto());

    private static ImmutableArray<ErrorDto> ToErrorDto(this ImmutableArray<Error> errors)
        => [..errors.Select(x => x.ToErrorDto())];

    private static ErrorDto ToErrorDto(this Error error)
        => new()
        {
            ErrorCode = error.ErrorCode,
            Message = error.Message,
        };
}