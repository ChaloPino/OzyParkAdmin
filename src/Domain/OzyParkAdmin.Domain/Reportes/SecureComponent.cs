namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// Un component seguro.
/// </summary>
/// <typeparam name="TSecure">El tipo del componente seguro.</typeparam>
public abstract class SecureComponent<TSecure> : ISecureComponent
    where TSecure : SecureComponent<TSecure>
{
    /// <summary>
    /// Los roles que tiene un componente seguro.
    /// </summary>
    public List<string>? Roles { get; protected set; }

    IEnumerable<string> ISecureComponent.Roles =>
        Roles is null
        ? ["*"]
        : [..Roles];
}
