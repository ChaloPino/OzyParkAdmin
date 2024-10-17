using OzyParkAdmin.Components.Admin.Shared;
using System.Diagnostics.CodeAnalysis;

namespace OzyParkAdmin.Components.Admin.Mantenedores.Seguridad.Usuarios.Models;

/// <summary>
/// Representa el view-model para el usuario.
/// </summary>
public sealed record UsuarioViewModel
{
    private string _userName = string.Empty;
    private string _friendlyName = string.Empty;
    private string? _rut = string.Empty;
    private string? _email = string.Empty;

    /// <summary>
    /// El id del usuario.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    /// El nombre del usuario.
    /// </summary>
    public string UserName
    {
        get => _userName;
        set => _userName = NormalizeValue(value, true);
    }

    /// <summary>
    /// El nombre completo del usuario.
    /// </summary>
    public string FriendlyName
    {
        get => _friendlyName;
        set => _friendlyName = NormalizeValue(value);
    }

    /// <summary>
    /// El rut del usiario.
    /// </summary>
    public string? Rut
    {
        get => _rut;
        set => _rut = NormalizeValue(value, true);
    }

    /// <summary>
    /// La dirección de correo electrónico del usuario.
    /// </summary>
    public string? Email
    {
        get => _email;
        set => _email = NormalizeValue(value, true);
    }

    /// <summary>
    /// La lista de roles.
    /// </summary>
    public IEnumerable<UsuarioRolModel> Roles { get; set; } = [];

    /// <summary>
    /// La lista de centros de costo.
    /// </summary>
    public IEnumerable<CentroCostoModel> CentrosCosto { get; set; } = [];

    /// <summary>
    /// La lista de franquicias.
    /// </summary>
    public IEnumerable<FranquiciaModel> Franquicias { get; set; } = [];

    /// <summary>
    /// Si el usuario está bloqueado.
    /// </summary>
    public bool IsLockedout { get; set; }

    /// <summary>
    /// Si el usuario es nuevo o no.
    /// </summary>
    public bool IsNew { get; set; }

    [return: NotNullIfNotNull(nameof(value))]
    private static string? NormalizeValue(string? value, bool removeInternalSpaces = false)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        ReadOnlySpan<char> span = value.AsSpan().Trim();

        if (!removeInternalSpaces)
        {
            return span.ToString();
        }

        Span<char> buffer = stackalloc char[span.Length];
        int index = 0;

        foreach (char c in span)
        {
            if (!char.IsWhiteSpace(c))
            {
                buffer[index++] = c;
            }
        }

        return new string(buffer.Slice(0, index));
    }
}
