using OzyParkAdmin.Domain.Reportes;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate;

/// <summary>
/// El proveedor de <see cref="IFormatReportGenerator"/>s.
/// </summary>
public interface IFormatReportGeneratorProvider
{
    /// <summary>
    /// Consigue el <see cref="IFormatReportGenerator"/> dependiendo del formato.
    /// </summary>
    /// <param name="format">El formato usado para conseguir el <see cref="IFormatReportGenerator"/>.</param>
    /// <returns>El <see cref="IFormatReportGenerator"/>.</returns>
    IFormatReportGenerator GetGenerator(ActionType format);
}