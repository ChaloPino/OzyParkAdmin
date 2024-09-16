using Microsoft.EntityFrameworkCore;
using OzyParkAdmin.Infrastructure.Identity;

namespace OzyParkAdmin.Infrastructure;

/// <summary>
/// Context de base de datos en Entity Framework usado por OzyParkAdmin.
/// </summary>
/// <remarks>
/// Crea una nueva instancia.
/// </remarks>
/// <param name="options">Las opciones a ser usadas por <see cref="OzyParkAdminContext"/>.</param>
public sealed class OzyParkAdminContext(DbContextOptions<OzyParkAdminContext> options) : DbContext(options)
{
    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OzyParkAdminContext).Assembly);
        //modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        //modelBuilder.ApplyConfiguration(new RolConfiguration());
        //modelBuilder.ApplyConfiguration(new ClaimUsuarioConfiguration());
        //modelBuilder.ApplyConfiguration(new UsuarioRolConfiguration());
        //modelBuilder.ApplyConfiguration(new UsuarioLoginConfiguration());
        //modelBuilder.ApplyConfiguration(new UsuarioTokenConfiguration());
        //modelBuilder.ApplyConfiguration(new CentroCostoUsuarioConfiguration());
        //modelBuilder.ApplyConfiguration(new FranquiciaUsuarioConfiguration());
    }
}
