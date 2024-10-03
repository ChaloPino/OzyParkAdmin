namespace OzyParkAdmin.Infrastructure.Seguridad.Permisos;


/// <summary>
/// Las acciones que se pueden hacer con las cajas.
/// </summary>
[Flags]
public enum CajaAcciones
{
    /// <summary>
    /// Ninguna acción.
    /// </summary>
    None = 0,

    /// <summary>
    /// Cerrar el día.
    /// </summary>
    CerrarDia = 1,

    /// <summary>
    /// Reabrir el día.
    /// </summary>
    ReabrirDia = 2,

    /// <summary>
    /// Cerrar un turno.
    /// </summary>
    CerrarTurno = 4,

    /// <summary>
    /// Reabrir un turno.
    /// </summary>
    ReabrirTurno = 8,

    /// <summary>
    /// Editar.
    /// </summary>
    Editar = 16,

    /// <summary>
    /// Visualizar el detalle del turno.
    /// </summary>
    VisualizarDetalleTurno = 32,

    /// <summary>
    /// Visualizar el detalle del turno cuando está cerrado.
    /// </summary>
    VisualizarDetalleTurnoCerrado = 64,
}