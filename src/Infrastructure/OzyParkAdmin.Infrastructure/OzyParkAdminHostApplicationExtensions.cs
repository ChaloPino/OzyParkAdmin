using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OzyParkAdmin.Infrastructure.Identity;
using OzyParkAdmin.Infrastructure;
using OzyParkAdmin.Application.Identity;
using MassTransit;
using OzyParkAdmin.Application;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Seguridad.Roles;
using OzyParkAdmin.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using OzyParkAdmin.Domain.Servicios;
using OzyParkAdmin.Infrastructure.Layout;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Qoi;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using OzyParkAdmin.Infrastructure.CatalogoImagenes;
using OzyParkAdmin.Domain.Productos;
using OzyParkAdmin.Domain.CatalogoImagenes;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contiene métodos de extensión para <see cref="IHostApplicationBuilder"/> para agregar el contexto y la infraestructura de OzyParkAdmin.
/// </summary>
public static class OzyParkAdminHostApplicationExtensions
{
    /// <summary>
    /// Agrega el contexto y la infraestructura al <see cref="IHostApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">El <see cref="IHostApplicationBuilder"/> que este método extiende.</param>
    /// <returns>El <see cref="IHostApplicationBuilder"/> que este método extiende.</returns>
    /// <exception cref="InvalidOperationException">Si no existe la cadena de conexión.</exception>
    public static IHostApplicationBuilder AddOzyParkAdmin(this IHostApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContextPool<IOzyParkAdminContext, OzyParkAdminContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<Usuario>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<Rol>()
            .AddClaimsPrincipalFactory<UsuarioClaimsPrincipalFactory>()
            .AddOzyParkAdminStores()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        builder.Services.AddAuthorization(options =>
            options.AddPolicy(AuthorizationConstants.PermissionPolicy, builder =>
                builder
                   .RequireAuthenticatedUser()
                   .AddRequirements(new PermissionRequirement())));

        builder.Services.AddSingleton<IEmailSender<Usuario>, IdentityNoOpEmailSender>();

        builder.Services.AddMediator(configure => 
            configure.AddConsumers(typeof(IOzyParkAdminContext).Assembly));

        builder.Services.AddServices();

        builder.Services.AddRepositories(typeof(OzyParkAdminHostApplicationExtensions).Assembly);

        return builder;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<LayoutService>();
        services.AddScoped<UsuarioService>();
        services.AddScoped<ServicioManager>();
        services.AddScoped<ServicioValidator>();
        services.AddScoped<ProductoManager>();
        services.AddScoped<ProductoValidator>();
        services.AddScoped<CatalogoImagenService>();
        services.AddSingleton(_ => CreateImageFormatManager());
        services.AddScoped<ImagenService>();
        return services;
    }

    private static ImageFormatManager CreateImageFormatManager()
    {
        ImageFormatManager manager = new();
        manager.AddImageFormat(PngFormat.Instance);
        manager.AddImageFormat(JpegFormat.Instance);
        manager.AddImageFormat(BmpFormat.Instance);
        manager.AddImageFormat(QoiFormat.Instance);
        manager.AddImageFormat(TgaFormat.Instance);
        manager.AddImageFormat(GifFormat.Instance);
        manager.AddImageFormat(PbmFormat.Instance);
        manager.AddImageFormat(TiffFormat.Instance);
        manager.AddImageFormat(WebpFormat.Instance);
        return manager;
    }
}
