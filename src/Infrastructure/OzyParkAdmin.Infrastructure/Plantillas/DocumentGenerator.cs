using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OzyParkAdmin.Domain.Cajas;
using OzyParkAdmin.Domain.Plantillas;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Domain.Tickets;
using OzyParkAdmin.Domain.Ventas;
using OzyParkAdmin.Infrastructure.Shared;
using System.Dynamic;

namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// La implementación del generador de documentos basados en plantillas.
/// </summary>
public class DocumentGenerator : IDocumentGenerator, IInfrastructure
{
    private readonly OzyParkAdminContext _context;
    private readonly HtmlGenerator _htmlGenerator;
    private readonly TemplateOptions _options;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DocumentGenerator"/>.
    /// </summary>
    /// <param name="context">El <see cref="OzyParkAdminContext"/>.</param>
    /// <param name="htmlGenerator">El <see cref="HtmlGenerator"/>.</param>
    /// <param name="options">Las <see cref="TemplateOptions"/>.</param>
    public DocumentGenerator(OzyParkAdminContext context, HtmlGenerator htmlGenerator, IOptions<TemplateOptions> options)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(htmlGenerator);
        ArgumentNullException.ThrowIfNull(options);
        _context = context;
        _htmlGenerator = htmlGenerator;
        _options = options.Value;
    }

    /// <inheritdoc/>>
    public async Task<ResultOf<byte[]>> GenerateTicketDocumentAsync(Ticket ticket, string ventaId, CancellationToken cancellationToken)
    {
        Plantilla? plantilla = await GetPlantillaByServiceIdAsync(ticket.Servicio.PlantillaId, cancellationToken);

        if (plantilla is null)
        {
            return new NotFound();
        }

        DocumentoImpresion? documentoImpresion = await FindDocumentoImpresionAsync(ticket.ImpresionId, cancellationToken);

        documentoImpresion ??= await GenerateDocumentoImpresionAsync(ticket, ventaId, plantilla, cancellationToken);

        IPdfGenerator pdfGenerator = PdfGeneratorFactory.Create(plantilla.EsLegado);

        return pdfGenerator.GenerateDocument(documentoImpresion.Contenido, documentoImpresion.TemplatePath, !plantilla.EsLegado ? "screen" : "zebra");
    }

    private async Task<DocumentoImpresion> GenerateDocumentoImpresionAsync(Ticket ticket, string ventaId, Plantilla plantilla, CancellationToken cancellationToken)
    {
        var info = await (from venta in _context.Set<Venta>()
                          join caja in _context.Set<Caja>() on venta.CajaId equals caja.Id
                          where venta.Id == ventaId
                          select new
                          {
                              Caja = caja.Aka,
                              PuntosVenta = caja.PuntoVenta.Nombre,
                              FranquiciaId = caja.Franquicia.Id,
                          }).FirstAsync(cancellationToken);

        ExpandoObject viewBag = new();
        viewBag.TryAdd("Caja", info.Caja);
        viewBag.TryAdd("Imagenes", new DataContainer<PlantillaImagen>(plantilla.Imagenes, i => i.Aka, CreateBase64));

        var ticketToPrint = new TicketToPrint
        {
            ParentId = ventaId,
            Id = ticket.Id,
            PuntoVenta = info.PuntosVenta,
            ImpresionId = ticket.ImpresionId,
            NumeroImpresion = ticket.NumeroImpresion,
            NumeroReimpresion = ticket.NumeroReimpresion,
            EsUsado = ticket.EsUsado,
            EsEnvioCorreo = ticket.EsEnvioCorreo,
            EsInformadoExterno = ticket.EsInformadoExterno,
            FechaImpresion = ticket.FechaImpresion,
            FechaReimpresion = ticket.FechaReimpresion,
            Fecha = ticket.Fecha,
            CantidadTramosTicket = ticket.CantidadTramosTicket,
            CantidasTramosUsados = ticket.CantidadTramosUsados,
            Servicio = ticket.Servicio,
            TramoId = ticket.TramoId,
            Tramo = ticket.Tramo,
            InicioVigencia = ticket.InicioVigencia,
            FinVigencia = ticket.FinVigencia,
            TipoAnulacion = ticket.TipoAnulacion,
            TipoPasajero = ticket.TipoPasajero,
            EsCortesia = ticket.EsCortesia,
            EsGuia = ticket.EsGuia,
            UsuarioNombre = ticket.Usuario.UserName,
            Codigo = ticket.Codigo,
            ZonaOrigenId = ticket.ZonaOrigenId,
            ZonaOrigen = ticket.ZonaOrigen,
            ZonaDesintoId = ticket.ZonaDestinoId,
            ZonaDestino = ticket.ZonaDestino,
            ZonaCupoOridenId = ticket.ZonaCupoOrigenId,
            ZonaCupoOrigen = ticket.ZonaCupoOrigen,
            SentidoId = ticket.SentidoId,
            Sentido = ticket.Sentido,
            CodigoFormato = ticket.CodigoFormato,
            TipoSegmentacionId = ticket.TipoSegmentacionId,
            TipoSegmentacion = ticket.TipoSegmenetacion,
            Detalles = ticket.Detalle
        };

        string html = await _htmlGenerator.GenerateHtmlAsync(ticketToPrint, plantilla, viewBag);

        DocumentoImpresion documentoImpresion = DocumentoImpresion.Create(
            ticket.ImpresionId,
            plantilla.Aka,
            plantilla.Impresora.Aka,
            plantilla.EsLegado,
            html,
            _options.TemplatePath);

        _context.Set<DocumentoImpresion>().Add(documentoImpresion);
        return documentoImpresion;
    }

    private async Task<DocumentoImpresion?> FindDocumentoImpresionAsync(string clave, CancellationToken cancellationToken) =>
        await _context.Set<DocumentoImpresion>().FirstOrDefaultAsync(x => x.Clave == clave, cancellationToken);

    private static string CreateBase64(PlantillaImagen imagen) =>
        $"data:{imagen.MimeType};base64,{Convert.ToBase64String(imagen.Contenido)}";

    private async Task<Plantilla?> GetPlantillaByServiceIdAsync(int plantillaId, CancellationToken cancellationToken) =>
        await _context.Set<Plantilla>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Impresora)
            .Include(x => x.Imagenes)
            .FirstOrDefaultAsync(x => x.Id == plantillaId, cancellationToken);
}
