using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Domain.CanalesVenta;

/// <summary>
/// El repositorio de <see cref="CanalVenta"/>
/// </summary>
public interface ICanalVentaRepository
{
    /// <summary>
    /// Busca los cananesl de venta que coincidan con los <paramref name="canalesVentaIds"/>
    /// </summary>
    /// <param name="canalesVentaIds">Los Id de los canales de venta</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>Lista de Canales</returns>
    Task<IEnumerable<CanalVenta>> FindByIdsAsync(int[] canalesVentaIds, CancellationToken cancellationToken);
}
