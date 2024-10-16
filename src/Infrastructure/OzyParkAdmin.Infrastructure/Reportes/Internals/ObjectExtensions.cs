namespace OzyParkAdmin.Infrastructure.Reportes.Internals;
internal static class ObjectExtensions
{
    public static string[] NotNullSplit(this string? value, char separator, StringSplitOptions options = StringSplitOptions.None)
        => value is not null ? value.Split(separator, options) : Array.Empty<string>();
}
