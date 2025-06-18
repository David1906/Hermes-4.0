using System.ComponentModel.DataAnnotations;

namespace Common;

public static class Validator
{
    public static bool TryValidate<T>(T obj, out List<ValidationResult> results) where T : class
    {
        var context = new ValidationContext(obj);
        results = [];
        return System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, context, results, true);
    }
}