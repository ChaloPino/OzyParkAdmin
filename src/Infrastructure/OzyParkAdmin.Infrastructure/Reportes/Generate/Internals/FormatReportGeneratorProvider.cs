using OzyParkAdmin.Domain.Reportes;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Html;
using OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pdf;
using OzyParkAdmin.Infrastructure.Shared;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals;

internal sealed class FormatReportGeneratorProvider : IFormatReportGeneratorProvider, IInfrastructure
{
    private readonly Dictionary<ActionType, IFormatReportGenerator> _reportGenerators = [];
    /// <summary>
    /// Crea una nueva instancia de <see cref="FormatReportGeneratorProvider"/>.
    /// </summary>
    public FormatReportGeneratorProvider()
    {

        _reportGenerators[ActionType.Html] = new HtmlReportGenerator();
        _reportGenerators[ActionType.Excel] = new ExcelReportGenerator();
        _reportGenerators[ActionType.Pdf] = new PdfReportGenerator();
    }

    /// <summary>
    /// Consigue el <see cref="FormatReportGenerator"/> dependiendo del formato.
    /// </summary>
    /// <param name="format">El formato usado para conseguir el <see cref="FormatReportGenerator"/>.</param>
    /// <returns>El <see cref="FormatReportGenerator"/>.</returns>
    public IFormatReportGenerator GetGenerator(ActionType format) =>
        _reportGenerators[format];
}