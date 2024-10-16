using OzyParkAdmin.Domain.Reportes.DataSources;

namespace OzyParkAdmin.Domain.Reportes.Filters;

/// <summary>
/// Define si es que un filtro puede conseguir información en forma remota, es decir desde una fuente de datos.
/// </summary>
public interface IRemoteFilter
{
    /// <summary>
    /// La fuente de datos para conseguir la información remota.
    /// </summary>
    DataSource? RemoteDataSource { get; }
}
