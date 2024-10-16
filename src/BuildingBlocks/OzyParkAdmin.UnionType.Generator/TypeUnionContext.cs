using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace OzyParkAdmin.UnionType.Generator;
internal sealed class TypeUnionContext
{
    private TypeUnionContext(
        INamedTypeSymbol symbol,
        ImmutableArray<string> typeParameters,
        ImmutableArray<ITypeSymbol> typeArguments,
        IReadOnlyDictionary<int, ITypeSymbol> originalSymbols,
        TypeUnionContext? parent)
    {
        Symbol = symbol;
        NameSpace = symbol.ContainingNamespace.ToDisplayString();
        Name = symbol.Name;
        Parameters = CreateParameters(typeParameters, typeArguments, originalSymbols);
        NameWithGenericTypes = $"{Name}{BuildOpenGenericPart(false)}";
        NameForDocumentation = $"{Name}{BuildOpenGenericPartForDocumentation()}";
        IsStruct = symbol.IsValueType;
        string classOrStruct = symbol.IsValueType ? "struct" : "class";
        TypeDefinition = $"partial {classOrStruct} {NameWithGenericTypes} : IEquatable<{NameWithGenericTypes}>";
        SupportsMatchTypeUnion = symbol.TypeArguments.Any();
        ConstructorArguments = BuildConstructorArguments();
        MatchTypeArguments = BuildMatchTypeArguments();
        SwitchArgumentsDocumentation = BuildSwitchArgumentsDocumentation(Parameters);
        MatchArgumentsDocumentation = BuildMatchArgumentsDocumentation(Parameters);
        MatchTypeArgumentsDocumentation = BuildTypeMatchArgumentsDocumentation(symbol, NameForDocumentation);
        Parent = parent;
    }

    public static TypeUnionContext? Create(
        INamedTypeSymbol symbol,
        Action<DiagnosticDescriptor>? reportDiagnosticDescriptor,
        TypeUnionContext? parentContext = null)
    {
        if (!symbol.ContainingSymbol.Equals(symbol.ContainingNamespace, SymbolEqualityComparer.Default))
        {
            reportDiagnosticDescriptor?.Invoke(GeneratorDiagnosticDescriptors.TopLevelError);
            return null;
        }

        AttributeData typeUnionAttribute = symbol.GetAttributes().First(ad =>
            ad.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)?.StartsWith($"global::{Resources.AttributeNameSpace}.{Resources.AttributeName}") ?? false);

        List<AttributeData> replaceAttributes = symbol.GetAttributes().Where(ad =>
            ad.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)?.StartsWith($"global::{Resources.AttributeNameSpace}.{Resources.ReplaceParameterAttributeName}") ?? false)
            .ToList();

        List<ITypeSymbol> typeArguments = [];
        List<string> typeParameters = [];
        Dictionary<int, ITypeSymbol> originalSymbols = [];

        if (symbol.IsGenericType)
        {
            typeArguments.AddRange(symbol.TypeArguments);
            typeParameters.AddRange(symbol.TypeParameters.Select(x => x.Name.ToPascalCase(1)));
        }

        if (typeArguments.Count == 0 && replaceAttributes.Count > 0)
        {
            reportDiagnosticDescriptor?.Invoke(GeneratorDiagnosticDescriptors.NonGenericClassMustNotImplementOneOf);
            return null;
        }

        if (typeArguments.Count < replaceAttributes.Count)
        {
            reportDiagnosticDescriptor?.Invoke(GeneratorDiagnosticDescriptors.ReplaceOfAttributeMustLessThanOrEqualToGenericArguments);
            return null;
        }

        foreach (AttributeData replaceAttribute in replaceAttributes)
        {
            string? memerName = replaceAttribute.ConstructorArguments[0].Value?.ToString();

            string? forName = null;
            INamedTypeSymbol? forType = null;

            foreach (var namedArgument in replaceAttribute.NamedArguments)
            {
                switch (namedArgument.Key.ToLowerInvariant())
                {
                    case "forname":
                        forName = namedArgument.Value.Value?.ToString();
                        break;
                    case "fortype":
                        forType = namedArgument.Value.Value as INamedTypeSymbol;
                        break;
                }
            }

            if (memerName is null || (forType is null && forName is null))
            {
                reportDiagnosticDescriptor?.Invoke(GeneratorDiagnosticDescriptors.ReplaceOfAttributeNotConfigured);
                return null;
            }

            if (forType?.IsUnboundGenericType == false)
            {
                reportDiagnosticDescriptor?.Invoke(GeneratorDiagnosticDescriptors.ReplaceOfAttributeForTypeMustBeGeneric);
                return null;
            }

            if (forType?.TypeArguments.Length > 1)
            {
                reportDiagnosticDescriptor?.Invoke(GeneratorDiagnosticDescriptors.ReplaceOfAttributeForTypeMustReplaceOneMember);
                return null;
            }

            int index = typeParameters.IndexOf(memerName);

            if (index < 0)
            {
                continue;
            }

            if (forType is not null)
            {
                ITypeSymbol originalSymbol = typeArguments[index];
                typeArguments.RemoveAt(index);
                typeArguments.Insert(index, forType);
                originalSymbols.Add(index, originalSymbol);
            }

            if (forName is not null)
            {
                typeParameters.RemoveAt(index);
                typeParameters.Insert(index, forName);
            }
        }

        typeArguments.AddRange(typeUnionAttribute.AttributeClass!.TypeArguments);

        for (int i = 0; i < typeUnionAttribute.AttributeClass.TypeArguments.Length; i++)
        {
            var namedArgument = typeUnionAttribute.NamedArguments.FirstOrDefault(x => x.Key.EndsWith($"{i}"));

            if (namedArgument.Key is null)
            {
                typeParameters.Add(typeUnionAttribute.AttributeClass.TypeArguments[i].ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                continue;
            }

            typeParameters.Add(namedArgument.Value.Value!.ToString()!);
        }

        foreach (var typeSymbol in typeArguments)
        {
            if (typeSymbol.Name == nameof(Object))
            {
                reportDiagnosticDescriptor?.Invoke(GeneratorDiagnosticDescriptors.ObjectIsOneOfType);
                return null;
            }

            if (typeSymbol.TypeKind == TypeKind.Interface)
            {
                reportDiagnosticDescriptor?.Invoke(GeneratorDiagnosticDescriptors.UserDefinedConversionsToOrFromAnInterfaceAreNotAllowed);
                return null;
            }
        }

        return new(symbol, [.. typeParameters], [.. typeArguments], originalSymbols, parentContext);
    }

    public TypeUnionContext? Parent { get; }

    public INamedTypeSymbol Symbol { get; }

    public bool IsStruct { get; }

    public string NameSpace { get; }

    public string Name { get; }

    public string TypeDefinition { get; }

    public string NameWithGenericTypes { get; }

    public string NameForDocumentation { get; }

    public string ConstructorArguments { get; }

    public IList<ParameterContext> Parameters { get; }

    public string MatchTypeArguments { get; }

    public string SwitchArgumentsDocumentation { get; }

    public string MatchArgumentsDocumentation { get; }

    public string MatchTypeArgumentsDocumentation { get; }

    public bool SupportsMatchTypeUnion { get; }

    public string GetPropertiesExcept(ParameterContext parameter, string propertyPrefix)
    {
        return Parameters.Except([parameter])
            .Aggregate(string.Empty, (acc, param) => string.IsNullOrEmpty(acc) ? $"nameof({param.GetProperty(propertyPrefix)})" : $"{acc}, nameof({param.GetProperty(propertyPrefix)})");
    }

    public List<(string Parameter, bool IsAsync, List<bool> AsyncPerArgument)> GetSwitchMethods()
    {
        List<(string, bool, List<bool>)> methods = [];

        for (int i = 0; i < (1 << Parameters.Count); i++)
        {
            List<string> arguments = [];
            List<bool> asyncPerArgument = [];

            for (int j = 0; j < Parameters.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    arguments.Add(GetActionAsyncArgument(Parameters[j]));
                    asyncPerArgument.Add(true);
                }
                else
                {
                    arguments.Add(GetActionArgument(Parameters[j]));
                    asyncPerArgument.Add(false);
                }
            }

            methods.Add((string.Join(", ", arguments), i > 0, asyncPerArgument));
        }

        return methods;
    }

    public static string GetSwitchMethod((string Parameter, bool IsAsync, List<bool> AsyncPerArgument) method, string onFunction, ParameterContext parameter, int index, int indent)
    {
        StringBuilder builder = new();

        return !method.IsAsync || !method.AsyncPerArgument[index]
            ? builder
                .Append(onFunction)
                .Append('(')
                .Append(parameter.RequiredFieldName)
                .Append(')')
                .Append(';')
                .AppendLine()
                .Append(' ', indent)
                .Append("return;")
                .ToString()
            : builder
                .Append("await ")
                .Append(onFunction)
                .Append('(')
                .Append(parameter.RequiredFieldName)
                .Append(", cancellationToken")
                .Append(')')
                .Append(".ConfigureAwait(false)")
                .Append(';')
                .AppendLine()
                .Append(' ', indent)
                .Append("return;")
                .ToString();
    }

    public List<(string Parameter, bool IsAsync, List<bool> AsyncPerArgument)> GetMatchMethods()
    {
        List<(string, bool, List<bool>)> methods = [];

        for (int i = 0; i < (1 << Parameters.Count); i++)
        {
            List<string> arguments = [];
            List<bool> asyncPerAgument = [];

            for (int j = 0; j < Parameters.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    arguments.Add(GetFunctionAsyncArgument(Parameters[j], false));
                    asyncPerAgument.Add(true);
                }
                else
                {
                    arguments.Add(GetFunctionArgument(Parameters[j], false));
                    asyncPerAgument.Add(false);
                }
            }

            methods.Add((string.Join(", ", arguments), i > 0, asyncPerAgument));
        }

        return methods;
    }

    public static string GetMatchMethod((string Parameters, bool IsAsync, List<bool> AsyncPerArgument) method, string onFunction, ParameterContext parameter, int index)
    {
        StringBuilder builder = new();

        builder.Append("return ");

        return !method.IsAsync || !method.AsyncPerArgument[index]
            ? builder
                .Append(onFunction)
                .Append('(')
                .Append(parameter.RequiredFieldName)
                .Append(')')
                .Append(';')
                .ToString()
            : builder
                .Append("await ")
                .Append(onFunction)
                .Append('(')
                .Append(parameter.RequiredFieldName)
                .Append(", cancellationToken")
                .Append(')')
                .Append(".ConfigureAwait(false)")
                .Append(';')
                .ToString();
    }

    public List<(string Parameter, bool IsAsync, List<bool> AsyncPerArgument)> GetMatchTypeUnionMethods()
    {
        List<(string, bool, List<bool>)> methods = [];

        for (int i = 0; i < (1 << Parameters.Count); i++)
        {
            List<string> arguments = [];
            List<bool> asyncPerArgument = [];

            for (int j = 0; j < Parameters.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    arguments.Add(GetFunctionAsyncArgument(Parameters[j], true));
                    asyncPerArgument.Add(true);
                }
                else
                {
                    arguments.Add(GetFunctionArgument(Parameters[j], true));
                    asyncPerArgument.Add(false);
                }
            }

            methods.Add((string.Join(", ", arguments), i > 0, asyncPerArgument));
        }

        return methods;
    }

    public string Coalesce(string defaultValue) =>
        Parameters.Any(x => !x.IsValueType) ? $" ?? {defaultValue}" : string.Empty;

    private static string GetActionArgument(ParameterContext parameter) =>
        $"Action<{parameter.GetRealType(false)}> {parameter.GetProperty("on")}";

    private static string GetActionAsyncArgument(ParameterContext parameter) =>
        $"Func<{parameter.GetRealType(false)}, CancellationToken, Task> {parameter.GetProperty("on")}";

    private string GetFunctionArgument(ParameterContext parameter, bool withTypeUnion) =>
        !withTypeUnion
            ? $"Func<{parameter.GetRealType(false)}, TResult> {parameter.GetProperty("on")}"
            : $"Func<{parameter.GetRealType(false)}, {Name}{BuildOpenGenericPart(true)}> {parameter.GetProperty("on")}";
    private string GetFunctionAsyncArgument(ParameterContext parameter, bool withTypeUnion) =>
        !withTypeUnion
            ? $"Func<{parameter.GetRealType(false)}, CancellationToken, Task<TResult>> {parameter.GetProperty("on")}"
            : $"Func<{parameter.GetRealType(false)}, CancellationToken, Task<{Name}{BuildOpenGenericPart(true)}>> {parameter.GetProperty("on")}";

    private static IList<ParameterContext> CreateParameters(
        ImmutableArray<string> typeParameters,
        ImmutableArray<ITypeSymbol> typeArguments,
        IReadOnlyDictionary<int, ITypeSymbol> originalSymbols)
    {
        var paramArgPairs =
            typeParameters.Zip(typeArguments, (param, args) => (param, args)).ToList();

        List<ParameterContext> parameters = [];

        for (int i = 0; i < paramArgPairs.Count; i++)
        {
            if (originalSymbols.TryGetValue(i, out ITypeSymbol? originalSymbol))
            {
                parameters.Add(new(paramArgPairs[i].param, paramArgPairs[i].args, originalSymbol));
                continue;
            }

            parameters.Add(new(paramArgPairs[i].param, paramArgPairs[i].args, null));
        }

        return parameters;
    }

    private string BuildConstructorArguments() =>
        string.Join(", ", Parameters.Select(x => x.ConstructorArgument));

    private string BuildMatchTypeArguments() =>
        Symbol.TypeArguments.Any()
        ? $"<{BuildGenericPart(Symbol.TypeArguments, true)}>"
        : string.Empty;

    private string? BuildOpenGenericPart(bool appendNew) =>
       !Symbol.TypeArguments.Any() ? null : $"<{BuildGenericPart(Symbol.TypeArguments, appendNew)}>";

    private static string BuildGenericPart(ImmutableArray<ITypeSymbol> typeArguments, bool appendNew) =>
        string.Join(", ", typeArguments.Select(x => BuildGenericArgumentName(x, appendNew)));

    private static string BuildGenericArgumentName(ITypeSymbol typeArgument, bool appendNew)
    {
        string name = $"{typeArgument.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}";

        if (appendNew)
        {
            name += "New";
        }

        return name;
    }

    private string? BuildOpenGenericPartForDocumentation() =>
        !Symbol.TypeArguments.Any() ? null : $"{{{BuildGenericPartForDocumentation(Symbol.TypeArguments)}}}";

    private static string BuildGenericPartForDocumentation(ImmutableArray<ITypeSymbol> typeArguments) =>
        string.Join(", ", typeArguments.Select(x => x.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).Replace('<', '{').Replace('>', '}')));

    private static string BuildSwitchArgumentsDocumentation(IList<ParameterContext> parameters)
    {
        StringBuilder builder = new();

        foreach (var parameter in parameters)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            builder.Append($@"    /// <param name=""{parameter.GetProperty("on")}"">the action that runs when the switch pattern finds has a value for {parameter.FieldName} member.</param>");
        }

        return builder.ToString();
    }

    private static string BuildMatchArgumentsDocumentation(IList<ParameterContext> parameters)
    {
        StringBuilder builder = new();

        foreach (var parameter in parameters)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            builder.Append($@"    /// <param name=""{parameter.GetProperty("on")}"">the function that runs when the matching pattern finds has a value for {parameter.FieldName} member.</param>");
        }

        return builder.ToString();
    }

    private static string BuildTypeMatchArgumentsDocumentation(INamedTypeSymbol symbol, string nameForDocumentation)
    {
        StringBuilder builder = new();

        foreach (var typeArgument in symbol.TypeArguments)
        {
            builder.Append($@"    /// <typeparam name=""{typeArgument.Name}New"">The first type of <see cref=""{nameForDocumentation}"" />.</typeparam>");
        }

        return builder.ToString();
    }

    public ParameterContext? CurrentParentParameter { get; private set; }
    public int CurrentParentIndex { get; private set; }
    internal void SetCurrentParameter(ParameterContext parameter, int index)
    {
        CurrentParentParameter = parameter;
        CurrentParentIndex = index;
    }
}
