using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OzyParkAdmin.Application.Identity;
using OzyParkAdmin.Infrastructure.Seguridad.Permisos;
using System.Reflection.Metadata.Ecma335;

namespace OzyParkAdmin.Infrastructure.Authorization;

/// <summary>
/// Manejador de autorización para permisos.
/// </summary>
public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly NavigationManager _navigationManager;
    private readonly IServiceScopeFactory _factory;

    /// <summary>
    /// Crea una nueva instancia de <see cref="PermissionAuthorizationHandler"/>.
    /// </summary>
    /// <param name="navigationManager">El <see cref="NavigationManager"/>.</param>
    /// <param name="factory">El <see cref="IServiceScopeFactory"/>.</param>
    public PermissionAuthorizationHandler(NavigationManager navigationManager, IServiceScopeFactory factory)
    {
        ArgumentNullException.ThrowIfNull(navigationManager);
        ArgumentNullException.ThrowIfNull(factory);
        _navigationManager = navigationManager;
        _factory = factory;
    }

    /// <inheritdoc/>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? resource = GetResource(context);

        if (resource is not null)
        {
            if (context.User.IsInRole(RolesConstant.SuperAdmin))
            {
                context.Succeed(requirement);
                return;
            }

            IEnumerable<PermisoRol> permisos = resource.StartsWith("group:", StringComparison.OrdinalIgnoreCase)
                ? await FindPermisoRolesGrupoAsync(resource.Replace("group:", string.Empty))
                : await FindPermisoRolesAsync(resource.Trim('/'));
            
            if (permisos.Any(x => context.User.IsInRole(x.Rol.Name)))
            {
                context.Succeed(requirement);
            }

            return;
        }

        if (context.Resource is HttpContext)
        {
            context.Succeed(requirement);
        }
    }

    private async Task<IEnumerable<PermisoRol>> FindPermisoRolesGrupoAsync(string grupo)
    {
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OzyParkAdminContext>();
        return await context.Set<PermisoRol>()
            .Where(x => x.Recurso.StartsWith(grupo))
            .ToListAsync();
    }

    private async Task<IEnumerable<PermisoRol>> FindPermisoRolesAsync(string recurso)
    {
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OzyParkAdminContext>();
        return await context.Set<PermisoRol>()
            .Where(x => x.Recurso == recurso)
            .ToListAsync();
    }

    private string? GetResource(AuthorizationHandlerContext context)
    {
        if (context.Resource is null)
        {
            return GetCurrentAddress();
        }

        return context.Resource is string resource ? resource : null;
    }

    private string? GetCurrentAddress()
    {
        Uri uri = new(_navigationManager.Uri);
        return uri.AbsolutePath;
    }
}
