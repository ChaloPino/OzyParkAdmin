﻿namespace OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios;

/// <summary>
/// Representa el view-model para el usuario.
/// </summary>
public sealed record UsuarioViewModel
{
    /// <summary>
    /// El id del usuario.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    /// El nombre del usuario.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// El nombre completo del usuario.
    /// </summary>
    public string FriendlyName { get; set; } = string.Empty;

    /// <summary>
    /// La dirección de correo electrónico del usuario.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// La lista de roles.
    /// </summary>
    public IEnumerable<UsuarioRolModel> Roles { get; set; } = [];

    /// <summary>
    /// La lista de centros de costo.
    /// </summary>
    public IEnumerable<UsuarioCentroCostoModel> CentrosCosto { get; set; } = [];

    /// <summary>
    /// La lista de franquicias.
    /// </summary>
    public IEnumerable<UsuarioFranquiciaModel> Franquicias { get; set; } = [];

    /// <summary>
    /// Si el usuario está bloqueado.
    /// </summary>
    public bool IsLockedout { get; set; }

    /// <summary>
    /// Si el usuario es nuevo o no.
    /// </summary>
    public bool IsNew { get; set; }
}
