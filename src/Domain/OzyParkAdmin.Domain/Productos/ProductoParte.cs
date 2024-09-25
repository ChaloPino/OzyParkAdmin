﻿namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Define un producto parte de un producto.
/// </summary>
public sealed class ProductoParte
{
    /// <summary>
    /// El producto parte de otro producto.
    /// </summary>
    public Producto Parte { get; private set; } = default!;

    /// <summary>
    /// La cantidad de la parte que se necesita para el producto.
    /// </summary>
    public decimal Cantidad { get; private set; }

    /// <summary>
    /// Si la parte es opcional para conformar el producto.
    /// </summary>
    public bool EsOpcional { get; private set; }
}