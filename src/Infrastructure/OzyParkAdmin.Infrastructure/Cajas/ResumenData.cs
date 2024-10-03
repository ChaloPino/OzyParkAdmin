namespace OzyParkAdmin.Infrastructure.Cajas;
internal sealed record ResumenData
{
    public Guid AperturaTurnoId { get; set; }
    public bool EsAbono { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Movimiento { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Monto { get; set; }
}
