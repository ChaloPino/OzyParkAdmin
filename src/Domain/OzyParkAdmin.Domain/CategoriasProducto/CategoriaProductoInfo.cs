﻿namespace OzyParkAdmin.Domain.CategoriasProducto;

/// <summary>
/// Contiene la información de una categoria de producto.
/// </summary>
public sealed class CategoriaProductoInfo
{
    /// <summary>
    /// El id de la categoría de producto.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// El aka de la categoria de producto.
    /// </summary>
    public string Aka { get; init; } = string.Empty;

    /// <summary>
    /// El nombre de la categoria de producto.
    /// </summary>
    public string Nombre { get; init; } = string.Empty;

    /// <summary>
    /// El nombre que se tiene que desplegar para las ediciones.
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Si la categoria de producto está activa.
    /// </summary>
    public bool EsActivo { get; init; }
}