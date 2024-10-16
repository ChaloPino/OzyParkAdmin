using NReco.PivotData;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;
internal static class PivotDataExtensions
{
    public static void ProcessData(this PivotData pivotData, IPivotDataSource source) =>
        source.ReadData(pivotData.ProcessData);
}
