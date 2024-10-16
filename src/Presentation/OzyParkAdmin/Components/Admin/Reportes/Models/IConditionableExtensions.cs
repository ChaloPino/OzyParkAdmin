using OzyParkAdmin.Domain.Reportes;
using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

internal static class IConditionableExtensions
{
    private readonly static Dictionary<ConditionalStyle, ConditionalStyleModel> ConditionalStyleCssMap = new()
    {
        { ConditionalStyle.Success, new(false, "mud-theme-success") },
        { ConditionalStyle.Warning, new(false, "mud-theme-warning") },
        { ConditionalStyle.Danger, new(false, "mud-theme-error") },
        { ConditionalStyle.Info, new(false, "mud-theme-info") },
        { ConditionalStyle.Light, new(true, "color: var(--mud-palette-black); background-color: var(--mud-palette-white)") },
        { ConditionalStyle.Dark, new(true, "color: var(--mud-palette-white); background-color: var(--mud-palette-black)") }
    };

    public static bool TryGenerateSuccessCssStyle(this IConditionable conditionable, [NotNullWhen(true)] out ConditionalStyleModel? style)
        => TryGenerateCssStyle(conditionable.SuccessStyle, out style);

    public static bool TryGenerateWarningCssStyle(this IConditionable conditionable, [NotNullWhen(true)] out ConditionalStyleModel? style)
        => TryGenerateCssStyle(conditionable.WarningStyle, out style);

    public static bool TryGenerateErrorCssStyle(this IConditionable conditionable, [NotNullWhen(true)] out ConditionalStyleModel? style)
        => TryGenerateCssStyle(conditionable.ErrorStyle, out style);

    private static bool TryGenerateCssStyle(ConditionalStyle? style, [NotNullWhen(true)] out ConditionalStyleModel? cssStyle)
    {
        cssStyle = null;

        if (!style.HasValue || style == ConditionalStyle.Default)
        {
            return false;
        }

        cssStyle = ConditionalStyleCssMap[style.Value];
        return true;
    }
}
