using Common.ResultOf;
using Core.Domain;
using Infrastructure.Data.Features.Errors;
using Infrastructure.Data.Features.Logfiles;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Features.Operations;

[Mapper]
public static partial class OperationMappers
{
    [MapperIgnoreSource(nameof(Operation.IsFailure))]
    [MapperIgnoreSource(nameof(Operation.Title))]
    [MapperIgnoreSource(nameof(Operation.TranslatedErrorType))]
    public static partial OperationDto ToDto(this Operation operation);

    private static LogfileDto ToDto(Logfile logfile) => logfile.ToDto();
    private static ErrorDto ToDto(Error error) => error.ToDto();

    public static partial Operation ToDomainModel(this OperationDto operationDto);

    private static Error ToDomainModel(ErrorDto errorDto) => errorDto.ToDomainModel();
}