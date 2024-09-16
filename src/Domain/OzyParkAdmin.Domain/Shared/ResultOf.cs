using TypeUnions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa el resultado del procesamiento.
/// </summary>
/// <typeparam name="TValue">El tipo del resultado en caso de éxito.</typeparam>
[TypeUnion<Failure>]
[ReplaceParameter("Value", ForName = "Success")]
public sealed partial class ResultOf<TValue>
{
    /// <summary>
    /// Implicitly casts a <see cref="ValidationError" /> to <see cref="Failure" />.
    /// </summary>
    /// <param name="_">The <see cref="ValidationError" /> to be casted into <see cref="ResultOf{TValue}" />.</param>
    /// <returns>
    /// A new instance of <see cref="ResultOf{TValue}" /> casted from <see cref="ValidationError" />.
    /// </returns>
    public static implicit operator ResultOf<TValue>(ValidationError _) => new(1, failure: _);
}
