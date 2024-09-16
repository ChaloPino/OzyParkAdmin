namespace OzyParkAdmin.Application.Seguridad.Usuarios.Lock;

/// <summary>
/// Bloquea a un usuario y retorna el resultado del bloqueo.
/// </summary>
/// <param name="UserId">El id del usuario a bloquear.</param>
public sealed record LockUser(Guid UserId) : IUserChangeable;
