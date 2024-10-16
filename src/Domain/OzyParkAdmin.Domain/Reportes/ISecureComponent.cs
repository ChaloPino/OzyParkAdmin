namespace OzyParkAdmin.Domain.Reportes;

/// <summary>
/// Componente que contiene los roles que se pueden ocupar.
/// </summary>
public interface ISecureComponent
{
    /// <summary>
    /// La lista de roles.
    /// </summary>
    IEnumerable<string> Roles { get; }
}