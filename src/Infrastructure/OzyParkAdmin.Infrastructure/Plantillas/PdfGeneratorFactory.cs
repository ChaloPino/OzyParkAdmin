namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// Factory de <see cref="IPdfGenerator"/>.
/// </summary>
public static class PdfGeneratorFactory
{
    /// <summary>
    /// Crea el <see cref="IPdfGenerator"/> dependiendo de si es legado o no.
    /// </summary>
    /// <param name="isLegacy">Si es legado o no.</param>
    /// <returns>Una nueva instancia de <see cref="IPdfGenerator"/>.</returns>
    public static IPdfGenerator Create(bool isLegacy) =>
        isLegacy
        ? new ZebraPdfGenerator()
        : new PdfGenerator();
}
