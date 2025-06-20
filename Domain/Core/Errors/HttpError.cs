using System.Collections.Immutable;
using System.Net;

namespace Domain.Core.Errors;

public class HttpError : Error
{
    public readonly ImmutableArray<Error> Errors;
    public readonly HttpStatusCode HttpStatusCode;

    public HttpError(ImmutableArray<Error> errors, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : base((int)httpStatusCode, string.Join("\n", errors.Select(e => e.Message)))
    {
        Errors = errors;
        HttpStatusCode = httpStatusCode;
    }
}