using Microsoft.CodeAnalysis;
using System.Text;

namespace OzyParkAdmin.UnionType.Generator;
internal static class Extensions
{
    public static string ToCamelCase(this string value, int skip = 0) =>
       value.Length == 1 ? value.ToLowerInvariant() : char.ToLowerInvariant(value[skip]) + value.Substring(skip + 1);

    public static string ToPascalCase(this string value, int skip = 0) =>
        value.Length == 1 ? value.ToUpperInvariant() : char.ToUpperInvariant(value[skip]) + value.Substring(skip + 1);


    public static string GetType(this ITypeSymbol type, ITypeSymbol? originalType, bool useNullable = true)
    {
        if (originalType is null)
        {
            StringBuilder builder = new(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

            if (useNullable && !type.IsValueType)
            {
                builder.Append('?');
            }

            return builder.ToString();
        }

        return GetTypeGeneric(type, originalType, useNullable);
    }

    public static string GetTypeGeneric(ITypeSymbol type, ITypeSymbol originalType, bool useNullable)
    {
        StringBuilder builder = new(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        builder.Remove(builder.Length - 1, 1);
        builder.Append(originalType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        builder.Append('>');

        if (useNullable && !type.IsValueType)
        {
            builder.Append('?');
        }

        return builder.ToString();
    }
}
