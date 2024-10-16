using OzyParkAdmin.Domain.Reportes.Charts;

namespace OzyParkAdmin.Application.Reportes.Charts;

/// <summary>
/// Las opciones del gráfico.
/// </summary>
public class ChartOptionsInfo : ChartDictionary
{
    internal static ChartOptionsInfo Create(Chart chart)
    {
        ChartOptionsInfo info = [];
        chart.Parsing.AddParsingTo(info);
        chart.Animations.AddDictionaryTo("animations", info);
        chart.Interaction.AddInteractionTo(info);
        chart.Layout.AddLayoutTo(info);
        chart.Responsive.AddTo("responsive", info);
        chart.MaintainAspectRatio.AddTo("maintainAspectRatio", info);
        chart.AspectRatio.AddTo("aspectRatio", info);
        chart.ResizeDelay.AddTo("resizeDelay", info);
        chart.Scales.AddDictionaryTo("scales", info);

        ChartPluginsInfo plugins = ChartPluginsInfo.Create(chart);

        if (plugins.Count > 0)
        {
            info.Add("plugins", plugins);
        }

        return info;
    }
}