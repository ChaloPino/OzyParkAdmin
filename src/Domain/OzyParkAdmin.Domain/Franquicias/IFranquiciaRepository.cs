namespace OzyParkAdmin.Domain.Franquicias;

/// <summary>
/// El repositorio de <see cref="Franquicia"/>.
/// </summary>
public interface IFranquiciaRepository
{
    /// <summary>
    /// Lista todas las franquicias activas que coincidan los ids.
    /// </summary>
    /// <param name="franquiciaIds">Los ids de franquicias.</param>
    /// <param name="cancellationToken">El <see cref="CancellationToken"/> usado para propagar notificaciones de que la operación debería ser cancelada.</param>
    /// <returns>El listado de franquicias activas.</returns>
    Task<List<Franquicia>> ListFranquiciasAsync(int[]? franquiciaIds, CancellationToken cancellationToken);
}
