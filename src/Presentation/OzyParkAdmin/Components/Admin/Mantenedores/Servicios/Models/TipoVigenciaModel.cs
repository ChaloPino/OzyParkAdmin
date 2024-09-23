using MassTransit.SagaStateMachine;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El tipo de vigencia de un servicio.
/// </summary>
public sealed record TipoVigenciaModel
{
    /// <summary>
    /// El id del tipo de vigencia.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// El aka del tipo de vigencia.
    /// </summary>
    public string Aka { get; set; } = string.Empty;

    /// <summary>
    /// El nombre del tipo de vigencia.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Si el tipo de vigencia está activo.
    /// </summary>
    public bool EsActivo { get; set; }

    /// <inheritdoc/>
    public override string ToString() =>
        string.Equals(Aka, "dd", StringComparison.OrdinalIgnoreCase) ? "día" : "hora";

    internal string ToLabel() =>
        string.Equals(Aka, "dd", StringComparison.OrdinalIgnoreCase) ? "Días" : "Hora";
}