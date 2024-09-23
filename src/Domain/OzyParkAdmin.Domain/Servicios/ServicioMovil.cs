
namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// Información adicional para un servicio que se puede mostrar en cajas móviles.
/// </summary>
public class ServicioMovil
{
    /// <summary>
    /// Si se puede mostrar los tramos.
    /// </summary>
    public bool MostrarTramos { get; private set; }

    internal static ServicioMovil Crear(bool mostraTramos)
    {
        return new() { MostrarTramos = mostraTramos };
    }

    internal void Update(bool mostrarTramos) =>
        MostrarTramos = mostrarTramos;
}