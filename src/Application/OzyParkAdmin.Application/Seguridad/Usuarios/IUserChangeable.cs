using OzyParkAdmin.Application.Shared;
using OzyParkAdmin.Domain.Seguridad.Usuarios;

namespace OzyParkAdmin.Application.Seguridad.Usuarios;

/// <summary>
/// Request para realizar cambios de estado del usuario.
/// </summary>
public interface IUserChangeable : ICommand<UsuarioFullInfo>;
