namespace OzyParkAdmin.Domain.Tramos;

/// <summary>
/// La entidad tramo.
/// </summary>
public class Tramo
{
    /// <summary>
    /// El id del tramo.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// El aka del tramo.
    /// </summary>
    public string Aka { get; private set; } = default!;

    /// <summary>
    /// La descripción del tramo.
    /// </summary>
    public string Descripcion { get; private set; } = default!;

    /// <summary>
    /// La cantidad de veces que se puede pasar por ese tramo.
    /// </summary>
    public int Cantidad { get; private set; }

    /// <summary>
    /// El tipo del tramo.
    /// </summary>
    public TipoTramo TipoTramo { get; private set; } = default!;

    /// <summary>
    /// Si el tramo es redondo.
    /// </summary>
    public bool EsRedondo { get; private set; }

    /// <summary>
    /// Si el tramo es cruzado.
    /// </summary>
    public bool EsCross { get; private set; }

    /// <summary>
    /// El orden en el que se muestra el tramo.
    /// </summary>
    public int Orden { get; private set; }
}