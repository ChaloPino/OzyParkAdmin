namespace OzyParkAdmin.Infrastructure.Cajas;
internal sealed record DetalleData
{
    public Guid AperturaTurnoId { get; set; }
    public bool EsAbono { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Correlativo { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; }
    public string FormaPagoAka { get; set; } = string.Empty;
    public string FormaPago { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public decimal MontoConSigno { get; set; }
    public string? NumeroReferencia { get; set; }
    public string? Supervisor { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public bool TieneAccion { get; set; }
    public string Movimiento { get; set; } = string.Empty;
    public int Orden { get; set; }
}
