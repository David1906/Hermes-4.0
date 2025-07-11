using Common.ResultOf;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Features.Errors;

[Mapper]
public static partial class ErrorMappers
{
    [MapperIgnoreSource(nameof(Error.TranslatedErrorType))]
    public static partial ErrorDto ToDto(this Error error);

    public static Error ToDomainModel(this ErrorDto errorDto)
    {
        try
        {
            var errorType = Type.GetType(errorDto.ErrorType);
            if (errorType != null)
            {
                return (Error)Activator.CreateInstance(errorType, errorDto.Message)!;
            }

            return new UnknownError(errorDto.Message);
        }
        catch
        {
            return new UnknownError(errorDto.Message);
        }
    }
}