namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// Agrupamiento de reportes.
/// </summary>
public sealed class ReportGroup
{
    private ReportGroup()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ReportGroup"/>.
    /// </summary>
    /// <param name="id">El identificador del agrupamiento.</param>
    public ReportGroup(int id)
    {
        Id = id;
    }

    /// <summary>
    /// El id del agrupamiento.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// El nombre del agrupamiento.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// El orden de despliegue del agrupamiento.
    /// </summary>
    public int? Order { get; private set; }

    public ReportGroup ChangeName(string name)
    {
        Name = name.Trim();
        return this;
    }

    public ReportGroup ChangeOrder(int? order)
    {
        Order = order;
        return this;
    }
}
