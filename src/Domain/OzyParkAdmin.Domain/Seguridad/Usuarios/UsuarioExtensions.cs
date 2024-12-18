﻿using OzyParkAdmin.Domain.CentrosCosto;
using OzyParkAdmin.Domain.Franquicias;
using OzyParkAdmin.Domain.Seguridad.Roles;

namespace OzyParkAdmin.Domain.Seguridad.Usuarios;

/// <summary>
/// Contiene métodos de extensión para <see cref="Usuario"/>.
/// </summary>
public static class UsuarioExtensions
{
    /// <summary>
    /// Convierte un <paramref name="usuario"/> a <see cref="UsuarioInfo"/>.
    /// </summary>
    /// <param name="usuario">El usuario a convertir.</param>
    /// <returns>Una nueva instancia de <see cref="UsuarioInfo"/> con los datos del <paramref name="usuario"/>.</returns>
    public static UsuarioInfo ToInfo(this Usuario usuario) =>
        new()
        {
            Id = usuario.Id,
            UserName = usuario.UserName,
            FriendlyName = usuario.FriendlyName,
        };

    /// <summary>
    /// Convierte un <paramref name="usuario"/> a <see cref="UsuarioFullInfo"/>.
    /// </summary>
    /// <param name="usuario">El usuario a convertir.</param>
    /// <returns>Una nueva instancia de <see cref="UsuarioFullInfo"/> con los datos del <paramref name="usuario"/>.</returns>
    public static UsuarioFullInfo ToFullInfo(this Usuario usuario) =>
        new()
        {
            Id = usuario.Id,
            UserName = usuario.UserName,
            FriendlyName = usuario.FriendlyName,
            Rut = usuario.Rut,
            Email = usuario.Email,
            EmailConfirmed = usuario.EmailConfirmed,
            IsLockedout = usuario.LockoutEndDateUtc is not null,
        };

    /// <summary>
    /// Convierte un <paramref name="usuario"/> a <see cref="UsuarioFullInfo"/>.
    /// </summary>
    /// <param name="usuario">El usuario a convertir.</param>
    /// <param name="roles">Roles asociados al usuario.</param>
    /// <param name="centrosCosto">Centros de costo asociados al usuario.</param>
    /// <param name="franquicia">Franquicias asociafos al usuario.</param>
    /// <returns>Una nueva instancia de <see cref="UsuarioFullInfo"/> con los datos del <paramref name="usuario"/>.</returns>
    public static UsuarioFullInfo ToFullInfo(this Usuario usuario, List<Rol> roles, List<CentroCosto> centrosCosto, List<FranquiciaInfo> franquicia)
    {
        UsuarioFullInfo usuarioInfo = usuario.ToFullInfo();
        usuarioInfo.Roles = roles;
        usuarioInfo.CentrosCosto = centrosCosto.ToInfo();
        usuarioInfo.Franquicias = franquicia;
        return usuarioInfo;
    }

    /// <summary>
    /// Convierte un <paramref name="usuario"/> a <see cref="UsuarioFullInfo"/>.
    /// </summary>
    /// <param name="usuario">El usuario a convertir.</param>
    /// <param name="roles">Roles asociados al usuario.</param>
    /// <param name="centrosCosto">Centros de costo asociados al usuario.</param>
    /// <param name="franquicia">Franquicias asociafos al usuario.</param>
    /// <returns>Una nueva instancia de <see cref="UsuarioFullInfo"/> con los datos del <paramref name="usuario"/>.</returns>
    public static UsuarioFullInfo ToFullInfo(this Usuario usuario, List<Rol> roles, List<CentroCostoInfo> centrosCosto, List<FranquiciaInfo> franquicia)
    {
        UsuarioFullInfo usuarioInfo = usuario.ToFullInfo();
        usuarioInfo.Roles = roles;
        usuarioInfo.CentrosCosto = centrosCosto;
        usuarioInfo.Franquicias = franquicia;
        return usuarioInfo;
    }
}
