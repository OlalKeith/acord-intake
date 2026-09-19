using System.Globalization;

namespace AcordIntake.Api.Parsing;

public static class ConversionTools
{
    public static string CheckForNull(string? value, string fallback)
    {
        return string.IsNullOrEmpty(value) ? fallback : value;
    }

    public static DateTime ConvertToDate(string? value, DateTime fallback)
    {
        if (string.IsNullOrWhiteSpace(value))
            return fallback;

        var normalized = value.Trim().Replace(" -", "-");
        return DateTime.TryParse(
            normalized,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces,
            out var result)
            ? DateTime.SpecifyKind(result, DateTimeKind.Utc)
            : DateTime.SpecifyKind(fallback, DateTimeKind.Utc);
    }

    public static decimal ConvertToDecimal(string? value, decimal fallback)
    {
        return decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result)
            ? result
            : fallback;
    }

    public static int ConvertToInt32(string? value, int fallback)
    {
        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
            ? result
            : fallback;
    }
}