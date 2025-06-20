using System.Collections.Immutable;
using System.Net;

namespace Domain.Core.Errors;

public class BadRequestError(
    ImmutableArray<Error> errors,
    HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    : HttpError(errors, httpStatusCode);