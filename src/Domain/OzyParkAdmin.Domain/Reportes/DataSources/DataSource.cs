namespace OzyParkAdmin.Domain.Reportes.DataSources;

/// <summary>
/// La fuente de datos.
/// </summary>
public sealed class DataSource
{
    private readonly List<Parameter> _parameters = [];

    /// <summary>
    /// Crea una nueva instancia de <see cref="DataSource"/>.
    /// </summary>
    public DataSource()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTimeOffset.Now;
        LastUpdate = DateTimeOffset.Now;
    }

    /// <summary>
    /// El id de la fuente de datos.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// El nombre de la fuente de datos.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// La cadena de conexión de la fuente de datos.
    /// </summary>
    public string ConnectionString { get; private set; } = default!;

    /// <summary>
    /// El nombre del proveedor de la fuente de datos.
    /// </summary>
    public string ProviderName { get; private set; } = default!;

    /// <summary>
    /// Si la fuente de datos es un prodcedimiento almacenado.
    /// </summary>
    public bool IsStoredProcedure { get; private set; }

    /// <summary>
    /// El script de la fuente de datos.
    /// </summary>
    public string Script { get; private set; } = default!;

    /// <summary>
    /// La fecha de creación de la fuente de datos.
    /// </summary>
    public DateTimeOffset CreationDate { get; private set; }

    /// <summary>
    /// La última fecha de actualización de la fuente de datos.
    /// </summary>
    public DateTimeOffset LastUpdate { get; private set; }

    /// <summary>
    /// El timestamp para la actualización de la fuente de datos.
    /// </summary>
    public byte[] Timestamp { get; private set; } = default!;

    /// <summary>
    /// La lista de parámetros de la fuente de datos.
    /// </summary>
    public IEnumerable<Parameter> Parameters => _parameters;
}
