using OzyParkAdmin.Domain.Reportes;
using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf;
internal static class IConditionableExtensions
{
    private readonly static Dictionary<ConditionalStyle, string> ConditionalStyleCssMap = new()
    {
        { ConditionalStyle.Success, "bg-success text-white" },
        { ConditionalStyle.Warning, "bg-warning text-dark" },
        { ConditionalStyle.Danger, "bg-danger text-white" },
        { ConditionalStyle.Info, "bg-info text-white" },
        { ConditionalStyle.Light, "bg-light text-dark" },
        { ConditionalStyle.Dark, "bg-dark text-white" }
    };
    public static bool TryGenerateSuccessCssStyle(this IConditionable conditionable, [NotNullWhen(true)] out string? @class)
        => TryGenerateCssStyle(conditionable.SuccessStyle, out @class);

    public static bool TryGenerateWarningCssStyle(this IConditionable conditionable, [NotNullWhen(true)] out string? @class)
        => TryGenerateCssStyle(conditionable.WarningStyle, out @class);

    public static bool TryGenerateErrorCssStyle(this IConditionable conditionable, [NotNullWhen(true)] out string? @class)
        => TryGenerateCssStyle(conditionable.ErrorStyle, out @class);

    private static bool TryGenerateCssStyle(ConditionalStyle? style, [NotNullWhen(true)] out string? @class)
    {
        @class = null;

        if (!style.HasValue || style == ConditionalStyle.Default)
        {
            return false;
        }

        @class = ConditionalStyleCssMap[style.Value];
        return true;
    }
}
