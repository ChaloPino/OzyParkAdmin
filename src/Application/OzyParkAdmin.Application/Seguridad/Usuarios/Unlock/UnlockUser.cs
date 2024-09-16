namespace OzyParkAdmin.Application.Seguridad.Usuarios.Unlock;

/// <summary>
/// Desbloquea a un usuario y retorna el resultado del desbloqueo.
/// </summary>
/// <param name="UserId">El id del usuario a desbloquear.</param>
public sealed record UnlockUser(Guid UserId) : IUserChangeable;
