using Microsoft.CodeAnalysis;
using System.Text;

namespace OzyParkAdmin.UnionType.Generator;
internal sealed class ParameterContext
{
    public ParameterContext(string name, ITypeSymbol type, ITypeSymbol? originalType)
    {
        Name = name;
        Type = type;
        OriginalType = originalType;
        FieldName = Name.ToCamelCase();
        RequiredFieldName = type.IsValueType ? FieldName : $"{FieldName}!";
        OptionalFieldName = type.IsValueType ? FieldName : $"{FieldName}?";
        TypeName = originalType is null
            ? type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
            : BuildMinimalTypeGeneric(type, originalType);
        FieldDeclaration = $"private readonly {GetRealType()} {FieldName}";
        AssignField = $"this.{FieldName} = {FieldName}";
        ConstructorArgument = BuildConstructorArgument();
        TypeDocumentation = type.TypeKind == TypeKind.TypeParameter
            ? $@"<typeparamref name=""{GetRealType(false).Replace('<', '{').Replace('>', '}')}"" />"
            : $@"<see cref=""{GetRealType(false).Replace('<', '{').Replace('>', '}')}"" />";
        IsTypeUnion = type.GetAttributes().Any(ad =>
            ad.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)?.StartsWith($"global::{Resources.AttributeNameSpace}.{Resources.AttributeName}") ?? false);
    }

    public string Name { get; }

    public ITypeSymbol Type { get; }

    public ITypeSymbol? OriginalType { get; }

    public string ConstructorArgument { get; }

    public string FieldDeclaration { get; }

    public string AssignField { get; }

    public string FieldName { get; }

    public string RequiredFieldName { get; }

    public string OptionalFieldName { get; }

    public string TypeName { get; }

    public string TypeDocumentation { get; }

    public bool IsTypeUnion { get; }

    public string GetRealType(bool useNullable = true) =>
            Type.GetType(OriginalType, useNullable);

    public string GetProperty(string prefix) =>
            $"{prefix}{Name.ToPascalCase()}";

    private string BuildConstructorArgument()
    {
        StringBuilder builder = new(Type.GetType(OriginalType));

        builder.Append($" {Name.ToCamelCase()}");

        if (Type.Kind == SymbolKind.TypeParameter || Type.IsValueType)
        {
            builder.Append(" = default");
        }
        else
        {
            builder.Append(" = null");
        }

        return builder.ToString();
    }

    private static string BuildMinimalTypeGeneric(ITypeSymbol type, ITypeSymbol originalType)
    {
        StringBuilder builder = new(type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
        builder.Remove(builder.Length - 1, 1);
        builder.Append(originalType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
        builder.Append('>');
        return builder.ToString();
    }
}
