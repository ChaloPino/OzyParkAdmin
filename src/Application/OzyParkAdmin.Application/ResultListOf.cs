using System.Collections.Immutable;

namespace OzyParkAdmin.Application;

/// <summary>
/// Resultado de listas de tipo <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">El tipo de los items de la lista.</typeparam>
/// <param name="Items">La lista de elementos.</param>
public sealed record ResultListOf<T>(ImmutableArray<T> Items)
{
    /// <summary>
    /// Convierte implícitamente un <see cref="ImmutableArray{T}"/> de tipo <typeparamref name="T"/> a <see cref="ResultListOf{T}"/>.
    /// </summary>
    public static implicit operator ResultListOf<T>(ImmutableArray<T> _) => new(_);

    /// <summary>
    /// Convierte implícitamente un arreglo de <typeparamref name="T"/> a <see cref="ResultListOf{T}"/>.
    /// </summary>
    public static implicit operator ResultListOf<T>(T[] _) => new([.. _]);

    /// <summary>
    /// Convierte implícitamente un <see cref="List{T}"/> de tipo <typeparamref name="T"/> a <see cref="ResultListOf{T}"/>.
    /// </summary>
    public static implicit operator ResultListOf<T>(List<T> _) => new([.._]);
}
