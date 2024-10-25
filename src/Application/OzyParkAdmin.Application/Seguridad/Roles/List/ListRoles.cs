using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Seguridad.Roles;
using System.Security.Claims;

namespace OzyParkAdmin.Application.Seguridad.Roles.List;

/// <summary>
/// Lista todos los roles hijo de los roles a los que pertenece el usuario que consulta.
/// </summary>
/// <param name="User">El usuario que consulta.</param>
public sealed record ListRoles(ClaimsPrincipal User) : IQueryListOf<Rol>;
