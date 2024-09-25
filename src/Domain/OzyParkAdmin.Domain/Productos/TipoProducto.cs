namespace OzyParkAdmin.Domain.Productos;

/// <summary>
/// Define el tipo de un producto.
/// </summary>
/// <param name="Id">El id del tipo de producto.</param>
/// <param name="Aka">El aka del tipo de producto.</param>
/// <param name="Nombre">El nombre del tipo de producto.</param>
/// <param name="ControlaStock">Si controla stock.</param>
/// <param name="ControlaInventario">Si controla inventario.</param>
/// <param name="EsParaVenta">Si es para venta.</param>
public sealed record TipoProducto(int Id, string Aka, string Nombre, bool ControlaStock, bool ControlaInventario, bool EsParaVenta);
