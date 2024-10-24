using MassTransit.Mediator;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Shared;

/// <summary>
/// Define un comando que devuelve un <see cref="ResultOf{TValue}"/>.
/// </summary>
/// <typeparam name="TResponse">El tipo de la respuesta.</typeparam>
public interface ICommand<TResponse> : Request<ResultOf<TResponse>>;

/// <summary>
/// Define un comando que devuelve un <see cref="SuccessOrFailure"/>.
/// </summary>
public interface ICommand : Request<SuccessOrFailure>;
