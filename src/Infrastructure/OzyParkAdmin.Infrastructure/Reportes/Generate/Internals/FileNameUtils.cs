using OzyParkAdmin.Application.Reportes.Generate;
using OzyParkAdmin.Domain.Reportes.Filters;
using OzyParkAdmin.Domain.Reportes;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals;
internal static partial class FileNameUtils
{
    private readonly static Regex _fileNamePatternRegex = FileNamePatternRegex();

    internal static string? ResolveFileName(Report report, ReportFilter filter, ReportTemplate? template) =>
        template is null || string.IsNullOrWhiteSpace(template.FileNamePattern)
        ? $"{report.Aka}_{DateTime.Today:yyyy_MM_dd}"
        : _fileNamePatternRegex.Replace(template.FileNamePattern, (match) => ReplaceReportFilters(match, report, filter));

    private static string ReplaceReportFilters(Match match, Report report, ReportFilter reportFilter)
    {
        bool hasValue = false;
        object? value = null;

        if (match.Value.StartsWith("@ReportAka", StringComparison.OrdinalIgnoreCase) ||
            match.Value.StartsWith("@ReportTitle", StringComparison.OrdinalIgnoreCase))
        {
            value = report.Aka;
            hasValue = true;
        }
        else
        {
            Filter? filter = report.Filters.FirstOrDefault(f => match.Value.StartsWith($"@{f.Name}", StringComparison.OrdinalIgnoreCase));

            if (filter is not null)
            {
                value = filter.GetText(reportFilter.GetFilter(filter.Id));
                hasValue = true;
            }
        }
        if (hasValue)
        {
            if (value == null)
            {
                return string.Empty;
            }
            if (match.Groups["StringFormat"] != null && !string.IsNullOrEmpty(match.Groups["StringFormat"].Value))
            {
                return string.Format(CultureInfo.CurrentCulture, match.Groups["StringFormat"].Value, value);
            }
            else if (match.Groups["ValueFormat"] != null && !string.IsNullOrEmpty(match.Groups["ValueFormat"].Value))
            {
                return string.Format(CultureInfo.CurrentCulture, string.Concat("{0", match.Groups["ValueFormat"].Value, "}"), value);
            }

            return value.ToString()!;
        }
        return match.Value;
    }

    [GeneratedRegex(@"(\@[a-zA-Z]+(\(((?<StringFormat>[0-9a-zA-Z_]+\{0\}[0-9a-zA-Z_]*)|(?<ValueFormat>:[0-9a-zA-Z_\-/\\':{}]+))\))?)", RegexOptions.IgnoreCase | RegexOptions.Compiled, "es-CL")]
    private static partial Regex FileNamePatternRegex();
}
