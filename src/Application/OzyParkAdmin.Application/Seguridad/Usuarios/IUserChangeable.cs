using MassTransit.Mediator;
using OzyParkAdmin.Domain.Seguridad.Usuarios;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Seguridad.Usuarios;

/// <summary>
/// Request para realizar cambios de estado del usuario.
/// </summary>
public interface IUserChangeable : Request<ResultOf<UsuarioInfo>>;
