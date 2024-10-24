using MassTransit.Mediator;
using OzyParkAdmin.Domain.Shared;

namespace OzyParkAdmin.Application.Shared;

/// <summary>
/// Query que devuelve como resultado un elemento de <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">El tipo de la respuesta.</typeparam>
public interface IQuery<TResponse> : Request<ResultOf<TResponse>>;

/// <summary>
/// Query que devuelve una lista paginada de <typeparamref name="TResponse"/> como respuesta.
/// </summary>
/// <typeparam name="TResponse">El tipo del elemento de la lista paginada.</typeparam>
public interface IQueryPagedOf<TResponse> : IQuery<PagedList<TResponse>>;


/// <summary>
/// Query que devuelve una lista de <typeparamref name="TResponse"/> como respuesta.
/// </summary>
/// <typeparam name="TResponse">El tipo del elemento de la lista.</typeparam>
public interface IQueryListOf<TResponse> : IQuery<List<TResponse>>;

/// <summary>
/// Query que devuelve un elemento <typeparamref name="TResponse"/> o nada.
/// </summary>
/// <typeparam name="TResponse">El tipo del elemento a ser devuelto.</typeparam>
public interface IQuerySomeOf<TResponse> : IQuery<SomeOrNone<TResponse>>;