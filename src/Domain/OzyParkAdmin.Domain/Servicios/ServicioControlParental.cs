namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información adicional para un servicio que tiene control parental.
/// </summary>
public sealed class ServicioControlParental
{
    /// <summary>
    /// El servicio que se define como responsable para los controles parentales.
    /// </summary>
    public int ServicioResponsableId { get; private set; }

    internal static ServicioControlParental Create(int servicioResponsableId) =>
        new() { ServicioResponsableId = servicioResponsableId };

    internal void Update(int servicioResponsableId) =>
        ServicioResponsableId = servicioResponsableId;
}