namespace OzyParkAdmin.Domain.CentrosCosto;

/// <summary>
/// La información del centro de costo.
/// </summary>
public sealed record CentroCostoInfo
{
    /// <summary>El id del centro de costo.</summary>
    public int Id { get; init; }

    /// <summary>La descripción del centro de costo.</summary>
    public string Descripcion { get; init; } = string.Empty;
}
