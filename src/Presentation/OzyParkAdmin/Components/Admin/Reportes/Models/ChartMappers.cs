using Nextended.Core.Helper;
using OzyParkAdmin.Domain.Reportes.Charts;

namespace OzyParkAdmin.Components.Admin.Reportes.Models;

internal static class ChartMappers
{
    public static string ToDescription(this ChartType type) =>
        type.ToDescriptionString().ToLowerInvariant();
}
