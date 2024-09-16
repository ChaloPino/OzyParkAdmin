using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OzyParkAdmin.Infrastructure.Shared;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OzyParkAdmin.Infrastructure;
internal static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            ScanAssembly(assembly, services);
        }

        return services;
    }

    private static void ScanAssembly(Assembly assembly, IServiceCollection services)
    {
        foreach (RepositoryDescritpor descriptor in assembly.DefinedTypes.Select(type => type.GetRepositoryDescriptor()).Where(descriptor => descriptor.IsRepository))
        {
            Type type = descriptor.Type;
            services.TryAddScoped(type);
            services.TryAddScoped(typeof(Repository<>).MakeGenericType(descriptor.EntityType!), sp => sp.GetRequiredService(type));

            foreach (var interfaceType in descriptor.InterfaceTypes)
            {
                services.TryAddScoped(interfaceType, sp => sp.GetRequiredService(type));
            }
        }
    }

    private static RepositoryDescritpor GetRepositoryDescriptor(this Type type)
    {
        if (!type.IsClass || type.IsAbstract || type.IsInterface)
        {
            return RepositoryDescritpor.NotRepository(type);
        }

        Type? baseType = type.BaseType;

        while (baseType is not null && baseType != typeof(object))
        {
            if (!baseType.IsGenericType || baseType.GetGenericTypeDefinition() != typeof(Repository<>))
            {
                baseType = baseType.BaseType;
                continue;
            }

            Type entityType = baseType.GetGenericArguments()[0];
            Type[] interfaceTypes = type.GetInterfaces();

            return new(type, true, entityType, interfaceTypes);
        }

        return RepositoryDescritpor.NotRepository(type);
    }


    private sealed record RepositoryDescritpor
    {
        public RepositoryDescritpor(Type type, bool isRepository, Type? entityType = null, params Type[] interfaceTypes)
        {
            Type = type;
            IsRepository = isRepository;
            EntityType = entityType;
            InterfaceTypes = new List<Type>(interfaceTypes);
        }

        public Type Type { get; }

        [MemberNotNullWhen(true, nameof(EntityType))]
        public bool IsRepository { get; }
        public Type? EntityType { get; }
        public List<Type> InterfaceTypes { get; }

        public static RepositoryDescritpor NotRepository(Type type) =>
            new(type, false);
    }
}
