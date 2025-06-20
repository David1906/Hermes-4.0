using Common.Languages;
using System.ComponentModel;

namespace Common.Extensions;

public static class EnumExtensions
{
    public static string ToUpperString(this Enum value)
    {
        return value.ToString().ToUpper();
    }

    public static IEnumerable<string> ValuesToTranslatedString<T>(bool includeAllOption = false) where T : Enum
    {
        return GetValues<T>()
            .Select(x => x.ToTranslatedString())
            .Prepend(includeAllOption ? Resources.txt_all : "")
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();
    }

    public static string ToTranslatedString(this Enum value)
    {
        return Resources.ResourceManager.GetString(value.ToResxString()) ?? value.ToString();
    }

    private static string ToResxString(this Enum value)
    {
        return "enum_" + value.ToString().ToLower();
    }

    public static string GetTranslatedDescription(this Enum value)
    {
        return Resources.ResourceManager.GetString(value.GetDescription()) ?? value.ToString();
    }

    public static string GetDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        var attributes =
            fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[] ??
            [];
        return attributes.Length != 0 ? attributes.First().Description : "";
    }

    public static List<string> GetEnumValues<T>() where T : Enum
    {
        return GetValues<T>().Select(x => x.ToString()).ToList();
    }

    public static T[] GetValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
    }

    public static T ParseOrDefault<T>(int value) where T : struct
    {
        return Enum.TryParse<T>(value.ToString(), out var parsedStatus) ? parsedStatus : default;
    }
}