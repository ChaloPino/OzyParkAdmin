using TypeUnions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Representa una falla en el procesamiento.
/// </summary>
[TypeUnion<Unknown, NotFound, Conflict, Forbid, Unauthorized, Validation>]
public readonly partial struct Failure
{
    /// <summary>
    /// Implicitly casts a <see cref="ValidationError" /> to <see cref="Failure" />.
    /// </summary>
    /// <param name="_">The <see cref="ValidationError" /> to be casted into <see cref="Failure" />.</param>
    /// <returns>
    /// A new instance of <see cref="Failure" /> casted from <see cref="ValidationError" />.
    /// </returns>
    public static implicit operator Failure(ValidationError _) => new(5, validation: new Validation([_]));

    /// <summary>
    /// Implicitly casts an array of <see cref="ValidationError" /> to <see cref="Failure" />.
    /// </summary>
    /// <param name="_">The array of <see cref="ValidationError" /> to be casted into <see cref="Failure" />.</param>
    /// <returns>
    /// A new instance of <see cref="Failure" /> casted from <see cref="ValidationError" />.
    /// </returns>
    public static implicit operator Failure(ValidationError[] _) => new(5, validation: new Validation([.._]));
}
