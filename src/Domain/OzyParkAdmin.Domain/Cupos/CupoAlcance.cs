namespace OzyParkAdmin.Domain.Cupos;

/// <summary>
/// El alcance para la búsqueda de cupos.
/// </summary>
public sealed record CupoAlcance
{
    /// <summary>
    /// Cuando es para venta.
    /// </summary>
    public static readonly CupoAlcance Venta = new("V", "Venta");

    /// <summary>
    /// Cuando es para revalidación.
    /// </summary>
    public static readonly CupoAlcance Revalidacion = new("R", "Revalidación");

    private CupoAlcance(string valor, string nombre) =>
        (Valor,  Nombre) = (valor, nombre);

    /// <summary>
    /// El valor del alcance.
    /// </summary>
    public string Valor { get; }

    /// <summary>
    /// El nombre del alcance.
    /// </summary>
    public string Nombre { get; }
}
