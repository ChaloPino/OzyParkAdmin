using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// El documento a ser impreso.
/// </summary>
public sealed class DocumentoImpresion
{
    /// <summary>
    /// El id del documento.
    /// </summary>
    public Guid Id { get; private init; }

    /// <summary>
    /// La fecha en que se generó el documento.
    /// </summary>
    public DateTime FechaImpresion { get; private init; } = DateTime.Now;

    /// <summary>
    /// La clave única del documento.
    /// </summary>
    public string Clave { get; private init; } = string.Empty;

    /// <summary>
    /// El tipo del documento.
    /// </summary>
    public string Tipo { get; private init; } = string.Empty;


    /// <summary>
    /// El nombre de la impresora.
    /// </summary>
    public string NombreImpresora { get; private init; } = string.Empty;


    /// <summary>
    /// Si el documento es legado.
    /// </summary>
    public bool EsLegado { get; private init; }

    /// <summary>
    /// El contenido del documento.
    /// </summary>
    public string Contenido { get; private init; } = string.Empty;


    /// <summary>
    /// La ruta de la plantilla.
    /// </summary>
    public string TemplatePath { get; private init; } = string.Empty;


    /// <summary>
    /// Crea un nuevo documento.
    /// </summary>
    /// <param name="clave">La clave del documento.</param>
    /// <param name="tipo">El tipo del documento.</param>
    /// <param name="nombreImpresora">El nombre de la impresora a usar para este documento.</param>
    /// <param name="esLegado">Si el documento es legado.</param>
    /// <param name="contenido">El contenido en html del documento.</param>
    /// <param name="templatePath">La ruta de la plantilla.</param>
    /// <returns>El <see cref="DocumentoImpresion"/> nuevo.</returns>
    public static DocumentoImpresion Create(
        string clave,
        string tipo,
        string nombreImpresora,
        bool esLegado,
        string contenido,
        string templatePath) =>
        new()
        {
            Id = Guid.NewGuid(),
            Clave = clave,
            Tipo = tipo,
            NombreImpresora = nombreImpresora,
            EsLegado = esLegado,
            Contenido = contenido,
            TemplatePath = templatePath
        };
}
