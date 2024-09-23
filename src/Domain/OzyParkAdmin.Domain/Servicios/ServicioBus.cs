
namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información adicional para los servicios de buses.
/// </summary>
public sealed class ServicioBus
{
    /// <summary>
    /// El tipo de servicio para buses.
    /// </summary>
    public TipoServicio IdaVuelta { get; private set; }

    internal static ServicioBus Create(TipoServicio idaVuelta)
    {
        return new() { IdaVuelta = idaVuelta };
    }

    internal void Update(TipoServicio idaVuelta) =>
        IdaVuelta = idaVuelta;
}