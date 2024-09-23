using System.Collections.Immutable;

namespace OzyParkAdmin.Application.Seguridad.Usuarios.Create;

/// <summary>
/// Crea un usuario.
/// </summary>
/// <param name="UserName">El nombre de usuario.</param>
/// <param name="FriendlyName">El nombre completo del usuario.</param>
/// <param name="Rut">El rut del usuario.</param>
/// <param name="Email">La dirección de correo electrónico del usuario.</param>
/// <param name="Roles">Los roles asociados al usuario.</param>
/// <param name="CentroCostos">Los centros de costo asociados al usuario.</param>
/// <param name="Franquicias">Las franquicias asociadas al usuario.</param>
public sealed record CreateUser(string UserName, string FriendlyName, string? Rut, string? Email, ImmutableArray<string> Roles, ImmutableArray<int> CentroCostos, ImmutableArray<int> Franquicias) : IUserChangeable;
