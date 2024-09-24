using TypeUnions;

namespace OzyParkAdmin.Domain.Shared;

/// <summary>
/// Define un resultado exitoso o con fallas.
/// </summary>

[TypeUnion<Success, Failure>]
public sealed partial class SuccessOrFailure
{
    /// <summary>
    /// Convierte implícitamente un <see cref="ValidationError"/> a <see cref="SuccessOrFailure"/>.
    /// </summary>
    /// <param name="_">El <see cref="ValidationError"/> a convertir.</param>
    public static implicit operator SuccessOrFailure(ValidationError _) => new Validation([_]);
}
