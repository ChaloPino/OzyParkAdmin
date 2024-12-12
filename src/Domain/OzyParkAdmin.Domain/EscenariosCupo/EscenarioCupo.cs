using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Zonas;

namespace OzyParkAdmin.Domain.EscenariosCupo;

/// <summary>
/// La entidad del escenario de cupo.
/// </summary>
public sealed class EscenarioCupo
{
    // Propiedades del escenario de cupo
    public int Id { get; private init; }
    public CentroCosto CentroCosto { get; private set; } = default!;
    public Zona? Zona { get; private set; } = default!;
    public string Nombre { get; private set; } = string.Empty;
    public bool EsHoraInicio { get; private set; }
    public int MinutosAntes { get; private set; }
    public bool EsActivo { get; private set; } = true;

    /// <summary>
    /// Crea un nuevo escenario de cupo.
    /// </summary>
    public static ResultOf<EscenarioCupo> Create(
        int id,
        CentroCosto centroCosto,
        Zona? zona,
        string nombre,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo)
    {
        // Validación básica
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return new ValidationError(nameof(Nombre), "El nombre no puede estar vacío.");
        }

        return new EscenarioCupo
        {
            Id = id,
            CentroCosto = centroCosto,
            Zona = zona,
            Nombre = nombre,
            EsHoraInicio = esHoraInicio,
            MinutosAntes = minutosAntes,
            EsActivo = esActivo
        };
    }

    /// <summary>
    /// Actualiza un escenario de cupo existente.
    /// </summary>
    public static ResultOf<EscenarioCupo> Update(
        EscenarioCupo existente,
        CentroCosto centroCosto,
        Zona? zona,
        string nombre,
        bool esHoraInicio,
        int minutosAntes,
        bool esActivo)
    {
        // Validación básica
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return new ValidationError(nameof(Nombre), "El nombre no puede estar vacío.");
        }

        existente.CentroCosto = centroCosto;
        existente.Zona = zona;
        existente.Nombre = nombre;
        existente.EsHoraInicio = esHoraInicio;
        existente.MinutosAntes = minutosAntes;
        existente.EsActivo = esActivo;

        return existente;
    }

    /// <summary>
    /// Marca un escenario de cupo como eliminado.
    /// </summary>
    public void MarcarComoEliminado()
    {
        EsActivo = false;
    }
}
