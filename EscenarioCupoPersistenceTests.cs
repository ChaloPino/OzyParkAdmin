[TestClass]
public class EscenarioCupoPersistenceTests
{
    private DbContextOptions<OzyParkAdminContext> _options;

    [TestInitialize]
    public void Setup()
    {
        // Configuración del DbContext en memoria
        _options = new DbContextOptionsBuilder<OzyParkAdminContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [TestMethod]
    public async Task CrearEscenarioCupoConDetalles_SinErroresDeClaves()
    {
        // Arrange
        await using var context = new OzyParkAdminContext(_options);

        var centroCosto = new CentroCosto { Id = 1, Descripcion = "Centro 1" };
        var servicio = new Servicio { Id = 1, Nombre = "Servicio 1" };
        await context.CentrosCosto.AddAsync(centroCosto);
        await context.Servicios.AddAsync(servicio);
        await context.SaveChangesAsync();

        // Crear EscenarioCupo
        var escenarioCupo = new EscenarioCupo
        {
            Id = 1,
            CentroCosto = centroCosto,
            Nombre = "Escenario Test",
            EsActivo = true
        };

        // Crear DetalleEscenarioCupo
        var detalle = new DetalleEscenarioCupo
        {
            EscenarioCupoId = escenarioCupo.Id,
            ServicioId = servicio.Id,
            TopeDiario = 10,
            HoraMaximaVenta = new TimeSpan(18, 0, 0),
            HoraMaximaRevalidacion = new TimeSpan(20, 0, 0),
            UsaSobreCupo = true,
            UsaTopeEnCupo = true
        };

        // Act
        await context.EscenariosCupo.AddAsync(escenarioCupo);
        await context.DetallesEscenarioCupo.AddAsync(detalle);
        await context.SaveChangesAsync();

        // Assert
        var savedEscenario = await context.EscenariosCupo
            .Include(e => e.DetallesEscenarioCupo)
            .FirstOrDefaultAsync(e => e.Id == escenarioCupo.Id);

        Assert.IsNotNull(savedEscenario);
        Assert.AreEqual(1, savedEscenario.DetallesEscenarioCupo.Count);
        Assert.AreEqual(detalle.TopeDiario, savedEscenario.DetallesEscenarioCupo.First().TopeDiario);
    }
}
