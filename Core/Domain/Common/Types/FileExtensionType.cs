using System.ComponentModel;

namespace Core.Domain.Common.Types;

public enum FileExtensionType
{
    [Description(".ret")] Ret,
    [Description(".3dx")] _3dx,
    [Description(".log")] Log,
    [Description(".rle")] Rle,
    [Description(".txt")] Txt,
    [Description(".res")] Res,
    [Description(".csv")] Csv
}