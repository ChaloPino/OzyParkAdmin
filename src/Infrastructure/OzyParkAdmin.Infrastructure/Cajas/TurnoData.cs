namespace OzyParkAdmin.Infrastructure.Cajas;
internal sealed record TurnoData
{
    public Guid AperturaCajaId { get; set; }
    public Guid AperturaTurnoId { get; set; }
    public string PuntoVenta { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string? Supervisor { get; set; }
    public int CajaId { get; set; }
    public string CajaAka { get; set; } = string.Empty;
    public int GavetaId { get; set; }
    public string GavetaAka { get; set; } = string.Empty;
    public DateTime FechaApertura { get; set; }
    public string? IpAddressApertura { get; set; }
    public string? IpAddressCierre { get; set; }
    public DateTime? FechaCierre { get; set; }
    public decimal EfectivoInicio { get; set; }
    public decimal? EfectivoCierreEjecutivo { get; set; }
    public decimal? MontoTransbankEjecutivo { get; set; }
    public decimal? EfectivoCierreSupervisor { get; set; }
    public decimal? MontoTransbankSupervisor { get; set; }
    public decimal? MontoVoucher { get; set; }
    public decimal? DiferenciaEfectivo { get; set; }
    public decimal? DiferenciaMontoTransbank { get; set; }
    public decimal? EfectivoCierreSistema { get; set; }
    public decimal? MontoTransbankSistema { get; set; }
    public string? Comentario { get; set; }
    public int Estado { get; set; }
    public int EstadoDia { get; set; }
}
