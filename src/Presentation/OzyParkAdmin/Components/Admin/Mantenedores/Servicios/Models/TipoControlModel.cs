namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios;

/// <summary>
/// El tipo de control de un servicio.
/// </summary>
public sealed record TipoControlModel
{
    /// <summary>
    /// El id del tipo de control.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El nombre del tipo de control.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Si el tipo de control está activo.
    /// </summary>
    public bool EsActivo { get; set; }
}