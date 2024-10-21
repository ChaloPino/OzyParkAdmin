using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OzyParkAdmin.Domain.Shared;
using OzyParkAdmin.Infrastructure.Shared;
using System.Reflection;

namespace OzyParkAdmin.Infrastructure;
internal static class RegistrationExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            ScanAssembly<IBusinessLogic>(assembly, services);
        }

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            ScanAssembly<IInfrastructure>(assembly, services);
        }

        return services;
    }

    private static void ScanAssembly<TMarker>(Assembly assembly, IServiceCollection services)
    {
        foreach (MarkerDescritpor descriptor in assembly.DefinedTypes.Select(type => type.GetMarkerDescriptor(typeof(TMarker))).Where(descriptor => descriptor.ImplementsMarker))
        {
            Type type = descriptor.Type;
            services.TryAddScoped(type);

            foreach (var interfaceType in descriptor.InterfaceTypes)
            {
                services.TryAddScoped(interfaceType, sp => sp.GetRequiredService(type));
            }
        }
    }

    private static MarkerDescritpor GetMarkerDescriptor(this Type type, Type markerType)
    {
        if (!type.IsClass || type.IsAbstract || type.IsInterface)
        {
            return MarkerDescritpor.NotMarker(type);
        }

        Type[] interfaceTypes = type.GetInterfaces();

        if (Array.Exists(interfaceTypes, x => x == markerType))
        {

            return new(type, true, [..interfaceTypes.Where(x => x != markerType)]);
        }

        return MarkerDescritpor.NotMarker(type);
    }


    private sealed record MarkerDescritpor
    {
        public MarkerDescritpor(Type type, bool implementsMarker, params Type[] interfaceTypes)
        {
            Type = type;
            ImplementsMarker = implementsMarker;
            InterfaceTypes = new List<Type>(interfaceTypes);
        }

        public Type Type { get; }

        public bool ImplementsMarker { get; }
        public List<Type> InterfaceTypes { get; }

        public static MarkerDescritpor NotMarker(Type type) =>
            new(type, false);
    }
}
