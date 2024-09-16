using OzyParkAdmin.Domain.Entidades;

namespace OzyParkAdmin.Domain.CentrosCosto;

/// <summary>
/// Emisor de un centro de costo.
/// </summary>
public sealed class EmisorCentroCosto
{
    /// <summary>
    /// Identificador del centro de costo.
    /// </summary>
    public int CentroCostoId { get; private set; }

    /// <summary>
    /// Rut del emisor.
    /// </summary>
    public string RutEmisor { get; private set; } = string.Empty;

    /// <summary>
    /// Datos del emisor.
    /// </summary>
    public DatosEmisor DatosEmisor { get; private set; } = default!;

    /// <summary>
    /// Si emite boletas afectas o no.
    /// </summary>
    public bool EsAfecto { get; private set; }

    /// <summary>
    /// Identificador del tipo de documento que puede emitir.
    /// </summary>
    public int TipoDocumentoId { get; private set; }

    /// <summary>
    /// Tipo de documento que puede emitir.
    /// </summary>
    public TipoDocumento TipoDocumento { get; private set; }

    /// <summary>
    /// Si el emisor está en uso o no.
    /// </summary>
    public bool EnUso { get; private set; }

    /// <summary>
    /// Si puede generar notas de crédito o no.
    /// </summary>
    public bool GeneraNotaCredito { get; private set; }

    /// <summary>
    /// Código del token del emisor.
    /// </summary>
    public string CodigoToken => DatosEmisor.CodigoToken;

    /// <summary>
    /// Razón social del emisor.
    /// </summary>
    public string RazonSocial => DatosEmisor.RazonSocial;

    /// <summary>
    /// Crea un nuevo emisor del centro de costo.
    /// </summary>
    /// <param name="centroCosto">Centro de costo.</param>
    /// <param name="datosEmisor">Datos del emisor.</param>
    /// <param name="esAfecto">Si es afecto.</param>
    /// <param name="tipoDocumento">Tipo documento.</param>
    /// <param name="enUso">Si está en uso.</param>
    /// <param name="generaNotaCredito">Si genera notas de crédito.</param>
    /// <returns>Un nuevo <see cref="EmisorCentroCosto"/>.</returns>
    internal static EmisorCentroCosto Crear(
        CentroCosto centroCosto,
        DatosEmisor datosEmisor,
        bool esAfecto,
        TipoDocumento tipoDocumento,
        bool enUso,
        bool generaNotaCredito) =>
        new()
        {
            CentroCostoId = centroCosto.Id,
            RutEmisor = datosEmisor.RutEmisor,
            EsAfecto = esAfecto,
            TipoDocumentoId = tipoDocumento.Id,
            TipoDocumento = tipoDocumento,
            EnUso = enUso,
            GeneraNotaCredito = generaNotaCredito
        };
}