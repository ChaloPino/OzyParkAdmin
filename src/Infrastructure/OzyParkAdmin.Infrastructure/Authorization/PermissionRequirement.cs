using Microsoft.AspNetCore.Authorization;

namespace OzyParkAdmin.Infrastructure.Authorization;

/// <summary>
/// Representa el requerimiento para la autorización por permisos.
/// </summary>
public sealed class PermissionRequirement : IAuthorizationRequirement
{
}