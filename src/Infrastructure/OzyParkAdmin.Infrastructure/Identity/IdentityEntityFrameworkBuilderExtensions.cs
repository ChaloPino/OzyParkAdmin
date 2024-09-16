using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OzyParkAdmin.Domain.Seguridad.Roles;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Infrastructure.Properties;
using OzyParkAdmin.Infrastructure.Seguridad.Roles;
using OzyParkAdmin.Infrastructure.Seguridad.Usuarios;

namespace OzyParkAdmin.Infrastructure.Identity;

/// <summary>
/// Contiene métodos de extensión para <see cref="IdentityBuilder"/> para agregar los almacenes de entity framework.
/// </summary>
public static class IdentityEntityFrameworkBuilderExtensions
{
    /// <summary>
    /// Agrega la implementación de Entity Framework para OzyParkAdmin de los almacenes de información de identidad.
    /// </summary>
    /// <param name="builder">La instancia de <see cref="IdentityBuilder"/> que este método extiende.</param>
    /// <returns>La instancia de <see cref="IdentityBuilder"/> que este método extiende.</returns>
    public static IdentityBuilder AddOzyParkAdminStores(this IdentityBuilder builder)
    {
        AddStores(builder.Services, builder.UserType, builder.RoleType);
        return builder;
    }

    private static void AddStores(IServiceCollection services, Type userType, Type? roleType)
    {
        var usuarioType = FindType(userType, typeof(Usuario));

        if (usuarioType is null)
        {
            throw new InvalidOperationException(Resources.NotIdentityUser);
        }

        if (roleType is not null)
        {
            var rolType = FindType(roleType, typeof(Rol));

            if (rolType is null)
            {
                throw new InvalidOperationException(Resources.NotIdentityRole);
            }

            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), typeof(UsuarioStore));
            services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), typeof(RoleStore));
        }
    }

    private static Type? FindType(Type currentType, Type baseType)
    {
        Type? type = currentType;

        while (type is not null)
        {
            if (type == baseType)
            {
                return type;
            }

            type = type.BaseType;
        }

        return null;
    }
}
