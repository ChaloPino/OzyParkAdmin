namespace OzyParkAdmin.Domain.Servicios;

/// <summary>
/// El grupo etario.
/// </summary>
/// <param name="Id">El id del grupo etario.</param>
/// <param name="Aka">El aka del grupo etario.</param>
/// <param name="Descripcion">La descripción del grupo etario.</param>
/// <param name="EdadDesde">La edad desde del grupo etario.</param>
/// <param name="EdadHasta">La edad hasta del grupo etario.</param>
/// <param name="RequiereSupervision">Si el grupo etario requiere supervisión.</param>
/// <param name="EsResponsable">Si el grupo etario puede ser responsable de un grupo.</param>
/// <param name="EsActivo">Si el grupo etario está activo.</param>
public sealed record GrupoEtario(int Id, string Aka, string Descripcion, int EdadDesde, int EdadHasta, bool RequiereSupervision, bool EsResponsable, bool EsActivo);
