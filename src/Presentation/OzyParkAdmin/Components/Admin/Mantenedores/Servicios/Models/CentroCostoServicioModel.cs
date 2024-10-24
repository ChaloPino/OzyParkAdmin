﻿using OzyParkAdmin.Domain.CentrosCosto;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Servicios.Models;

/// <summary>
/// El nombre que tiene el servicio para un centro de costo.
/// </summary>
public sealed record CentroCostoServicioModel
{
    /// <summary>
    /// El centro de costo asociado.
    /// </summary>
    public CentroCostoInfo CentroCosto { get; set; } = default!;

    /// <summary>
    /// El nombre que reemplaza.
    /// </summary>
    public string? Nombre { get; set; }
}