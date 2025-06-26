using Domain.Logfiles;
using Domain.Operations;
using Infrastructure.Data.Logfiles;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Operations;

[Mapper]
public static partial class OperationsMappers
{
    public static partial OperationDbModel ToDbModel(this Operation operation);

    [MapperIgnoreSource(nameof(Logfile.FileNameWithoutExtension))]
    [MapperIgnoreSource(nameof(Logfile.Content))]
    [MapperIgnoreSource(nameof(Logfile.IsEmpty))]
    [MapperIgnoreSource(nameof(Logfile.FullName))]
    [MapperIgnoreSource(nameof(Logfile.Name))]
    [MapperIgnoreSource(nameof(Logfile.Exists))]
    [MapProperty(nameof(Logfile.FileInfo), nameof(LogfileDbModel.FileInfo), Use = nameof(MapFileInfo))]
    public static partial LogfileDbModel ToDbModel(this Logfile operation);

    private static string MapFileInfo(FileInfo fileInfo)
        => fileInfo.FullName;

    public static partial Operation ToDomainModel(this OperationDbModel operationDbModel);

    [MapperIgnoreTarget("Content")]
    [MapProperty(nameof(LogfileDbModel.FileInfo), nameof(Logfile.FileInfo), Use = nameof(MapStringToFileInfo))]
    public static partial Logfile ToDomainModel(this LogfileDbModel logfileDbModel);

    private static FileInfo MapStringToFileInfo(string fullPath)
        => new FileInfo(fullPath);
}