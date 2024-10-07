using System.Reflection;

namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// Proveedor de ensamblados para el uso del <see cref="RazorTemplate{TModel}"/>
/// </summary>
public class AssemblyProvider
{
    private readonly List<Assembly> _assemblyList = [];

    /// <summary>
    /// Agrega ensamblados para el uso de la generación de html.
    /// </summary>
    /// <param name="assemblies">Lista de ensamblados.</param>
    public AssemblyProvider AddAssemblies(params Assembly[] assemblies)
    {
        if (assemblies is { Length: > 0 })
        {
            _assemblyList.AddRange(assemblies);
        }

        return this;
    }

    /// <summary>
    /// Consigue todos los ensamblados.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Assembly> GetAssemblies() =>
        _assemblyList;
}