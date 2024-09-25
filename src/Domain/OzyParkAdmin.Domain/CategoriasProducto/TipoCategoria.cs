namespace OzyParkAdmin.Domain.CategoriasProducto;

/// <summary>
/// El tipo de categoría.
/// </summary>
public enum TipoCategoria
{
    /// <summary>
    /// Categoría padre.
    /// </summary>
    Padres = 0,

    /// <summary>
    /// Categoría no final.
    /// </summary>
    Intermedias = 1,

    /// <summary>
    /// Categoria final.
    /// </summary>
    Finales = 2,

    /// <summary>
    /// Todos los tipos de categoría.
    /// </summary>
    Todas = 3,
}