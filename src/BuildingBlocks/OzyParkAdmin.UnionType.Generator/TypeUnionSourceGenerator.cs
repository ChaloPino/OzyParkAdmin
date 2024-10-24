﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;

namespace OzyParkAdmin.UnionType.Generator;

/// <summary>
/// The source generator of type unions
/// </summary>
[Generator]
public class TypeUnionSourceGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            for (int i = 1; i <= 32; i++)
            {
                ctx.AddSource($"{Resources.TypeUnionName}T{i}.g.cs", Resources.GenerateAttribute(i));
            }

            ctx.AddSource($"{Resources.FunctionClassName}.g.cs", Resources.FunctionText);
            ctx.AddSource($"{Resources.ReplaceParameterAttributeName}.g.cs", Resources.ReplaceParameterAttributeText);
        });

        Dictionary<int, IncrementalValueProvider<ImmutableArray<INamedTypeSymbol?>>> dictionary = [];

        for (int i = 1; i <= 32; i++)
        {
            dictionary[i] = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: $"{Resources.AttributeNameSpace}.{Resources.AttributeName}`{i}",
                predicate: static (s, _) => IsSyntaxTargetForGenerator(s),
                transform: static (ctx, _) => GetSemanticTargetForGenerator(ctx))
            .Where(static m => m is not null)
            .Collect();
        }

        foreach (var value in dictionary.Values)
        {
            context.RegisterSourceOutput(value, Execute);
        }

        static bool IsSyntaxTargetForGenerator(SyntaxNode node) =>
            node is TypeDeclarationSyntax { AttributeLists.Count: > 0 } typedSyntax &&
                typedSyntax.Modifiers.Any(SyntaxKind.PartialKeyword);

        static INamedTypeSymbol? GetSemanticTargetForGenerator(GeneratorAttributeSyntaxContext context)
        {
            var symbol = context.TargetSymbol;

            if (symbol is not INamedTypeSymbol namedTypeSymbol)
            {
                return null;
            }

            var attributeData = namedTypeSymbol.GetAttributes().FirstOrDefault(ad =>
                ad.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith($"global::{Resources.AttributeNameSpace}.{Resources.AttributeName}") ?? false);

            return attributeData is null ? null : namedTypeSymbol;
        }
    }

    private static void Execute(SourceProductionContext context, ImmutableArray<INamedTypeSymbol?> symbols)
    {
        foreach (var namedTypeSymbol in symbols.Where(symbol => symbol is not null))
        {
            string? classSource = ProcessClass(namedTypeSymbol!, context);

            if (classSource is null)
            {
                continue;
            }

            context.AddSource($"{namedTypeSymbol!.ContainingNamespace}_{namedTypeSymbol.Name}.g.cs", classSource);
        }
    }

    private static string? ProcessClass(INamedTypeSymbol symbol, SourceProductionContext context)
    {
        var attributeLocation = symbol.Locations.FirstOrDefault() ?? Location.None;

        TypeUnionContext? typeUnionContext = TypeUnionContext.Create(symbol, CreateDiagnosticError);

        return typeUnionContext is null ? null : GenerateClassSource(typeUnionContext);
        void CreateDiagnosticError(DiagnosticDescriptor descriptor) =>
            context.ReportDiagnostic(Diagnostic.Create(descriptor, attributeLocation, symbol.Name, DiagnosticSeverity.Error));
    }

    private static string GenerateClassSource(
       TypeUnionContext context)
    {
        string nameWithGenericTypes = context.NameWithGenericTypes;
        string nameForDocumenation = context.NameForDocumentation;

        StringBuilder source = new();
        BuildHeaderAndDeclaration(context, source);
        BuildFields(context, source);
        BuildConstructor(context, source);
        BuildSwitchMethods(context, nameForDocumenation, source);
        BuildMatchMethods(context, nameForDocumenation, source);

        if (context.HasGenericArguments)
        {
            BuildBindMethods(context, nameForDocumenation, source);
        }

        BuildIsMethods(context, nameForDocumenation, source);
        BuildEqualityMethods(context, nameWithGenericTypes, source);
        BuildGetHashMethod(context, source);

        if (!context.HasStringMethodOverride)
        {
            BuildToStringMethod(context, source);
        }

        BuildImplicitOperators(context, nameWithGenericTypes, nameForDocumenation, source);
        BuildOperators(context, nameWithGenericTypes, source);
        source.Append('}').AppendLine();
        return source.ToString();
    }

    private static void BuildHeaderAndDeclaration(TypeUnionContext context, StringBuilder source)
    {
        source.Append($@"// <auto-generated />
#pragma warning disable 1591
#nullable enable

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using static {Resources.AttributeNameSpace}.{Resources.FunctionClassName};

namespace {context.NameSpace};
{context.TypeDefinition}
{{");
    }

    private static void BuildImplicitOperators(TypeUnionContext context, string nameWithGenericTypes, string nameForDocumenation, StringBuilder source)
    {
        // Implicit operators
        int index = 0;

        if (context.Parent is null)
        {
            foreach (var parameter in context.Parameters)
            {
                source.Append($"""

    /// <summary>
    /// Implicitly casts a {parameter.TypeDocumentation} to <see cref="{nameForDocumenation}" />.
    /// </summary>
    /// <param name="_">The {parameter.TypeDocumentation} to be casted into <see cref="{nameForDocumenation}" />.</param>
    /// <returns>
    /// A new instance of <see cref="{nameForDocumenation}" /> casted from {parameter.TypeDocumentation}.
    /// </returns>
    public static implicit operator {nameWithGenericTypes}({parameter.GetRealType(false)} _) => new {nameWithGenericTypes}({index++}, {parameter.FieldName}: _);

""");
            }
        }
        else
        {
            foreach (var parameter in context.Parameters)
            {
                source.Append($"""

    /// <summary>
    /// Implicitly casts a {parameter.TypeDocumentation} to <see cref="{nameForDocumenation}" />.
    /// </summary>
    /// <param name="_">The {parameter.TypeDocumentation} to be casted into <see cref="{nameForDocumenation}" />.</param>
    /// <returns>
    /// A new instance of <see cref="{nameForDocumenation}" /> casted from {parameter.TypeDocumentation}.
    /// </returns>
    public static implicit operator {nameWithGenericTypes}({parameter.GetRealType(false)} _) => new {nameWithGenericTypes}({context.CurrentParentIndex}, {context.CurrentParentParameter!.FieldName}: _);

""");
            }
        }

        index = 0;

        foreach (var parameter in context.Parameters)
        {
            if (parameter.IsTypeUnion)
            {
                var parameterContext = TypeUnionContext.Create((INamedTypeSymbol)parameter.Type, null, context);

                if (parameterContext is not null)
                {
                    parameterContext.SetCurrentParameter(parameter, index);
                    BuildImplicitOperators(parameterContext, nameWithGenericTypes, nameForDocumenation, source);
                }
            }

            index++;
        }
    }

    private static void BuildOperators(TypeUnionContext context, string nameWithGenericTypes, StringBuilder source)
    {
        // Equality operators
        if (context.IsStruct)
        {
            source.Append($@"
    public static bool operator ==({nameWithGenericTypes} left, {nameWithGenericTypes} right) => left.Equals(right);
    public static bool operator !=({nameWithGenericTypes} left, {nameWithGenericTypes} right) => !(left == right);
");
        }
    }

    private static void BuildToStringMethod(TypeUnionContext context, StringBuilder source)
    {
        // ToString Method
        @source.Append(@"
    /// <inheritdoc/>
    public override string ToString() =>
        index switch
        {");

        int index = 0;

        foreach (string fieldName in context.Parameters.Select(x => x.FieldName))
        {
            source.Append($@"
            {index++} => FormatValue({fieldName}),");
        }

        @source.Append("""

            _ => string.Empty,
        };
""");
    }

    private static void BuildGetHashMethod(TypeUnionContext context, StringBuilder source)
    {
        // GetHashCode Method
        @source.Append(@"
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = index switch
            {");

        int index = 0;

        foreach (var parameter in context.Parameters)
        {
            source.Append($@"
                {index++} => {parameter.OptionalFieldName}.GetHashCode(),");
        }

        @source.Append($@"
                _ => 0
            }}{context.Coalesce("0")};
                
            return (hashCode * 397) ^ index;
        }}
    }}
");
    }

    private static void BuildEqualityMethods(TypeUnionContext context, string nameWithGenericTypes, StringBuilder source)
    {
        // Equality Methods
        @source.Append($@"
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is {nameWithGenericTypes} other && Equals(other);
");
        if (context.IsStruct)
        {
            source.Append($@"
    /// <inheritdoc/>
    public bool Equals({nameWithGenericTypes} other) => EqualsMembers(other);
");
        }
        else
        {
            source.Append($@"
    /// <inheritdoc/>
    public bool Equals({nameWithGenericTypes}? other) => other is not null && EqualsMembers(other);
");
        }

        source.Append($@"
    private bool EqualsMembers({nameWithGenericTypes} other) =>
        index == other.index &&
        index switch
        {{");

        int index = 0;

        foreach (string fieldName in context.Parameters.Select(x => x.FieldName))
        {
            source.Append($@"
            {index++} => Equals({fieldName}, other.{fieldName}),");
        }

        source.Append(@"
            _ => false
        };
");
    }

    private static void BuildIsMethods(TypeUnionContext context, string nameForDocumenation, StringBuilder source)
    {
        // Is Methods

        int index = 0;

        if (context.Parent is null)
        {
            foreach (var parameter in context.Parameters)
            {
                source.Append($$"""

    /// <summary>
    /// Try to get the {{parameter.FieldName}} member if exists.
    /// </summary>
    /// <param name="{{parameter.FieldName}}">
    /// When this method returns, contains the value associated with the {{parameter.FieldName}} member,
    /// if the {{parameter.FieldName}} is found; otherwise, the default value for the type of the value parameter.
    /// This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    /// <c>true</c> if the <see cref="{{nameForDocumenation}}" /> is the {{parameter.FieldName}} member; otherwise, <c>false</c>.
    /// </returns>
    public bool {{parameter.GetProperty("Is")}}([NotNullWhen(true)]out {{parameter.GetRealType()}} {{parameter.FieldName}})
    {

""")
                    .Append($@"
        if (index == {index})
        {{
            {parameter.FieldName} = this.{parameter.RequiredFieldName};
            return true;
        }}

        {parameter.FieldName} = default;
        return false;
    }}
");
                index++;
            }
        }
        else
        {
            foreach (var parameter in context.Parameters)
            {
                source.Append($$"""

    /// <summary>
    /// Try to get the {{parameter.FieldName}} member if exists.
    /// </summary>
    /// <param name="{{parameter.FieldName}}">
    /// When this method returns, contains the value associated with the {{parameter.FieldName}} member,
    /// if the {{parameter.FieldName}} is found; otherwise, the default value for the type of the value parameter.
    /// This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    /// <c>true</c> if the <see cref="{{nameForDocumenation}}" /> is the {{parameter.FieldName}} member; otherwise, <c>false</c>.
    /// </returns>
    public bool {{parameter.GetProperty("Is")}}([NotNullWhen(true)]out {{parameter.GetRealType()}} {{parameter.FieldName}})
    {

""")
                    .Append($@"
        if (index == {context.CurrentParentIndex} && this.{context.CurrentParentParameter!.FieldName}.{parameter.GetProperty("Is")}(out {parameter.FieldName}))
        {{
            return true;
        }}

        {parameter.FieldName} = default;
        return false;
    }}
");
            }
        }

        index = 0;

        foreach (var parameter in context.Parameters)
        {
            if (parameter.IsTypeUnion)
            {
                var parameterContext = TypeUnionContext.Create((INamedTypeSymbol)parameter.Type, null, context);

                if (parameterContext is not null)
                {
                    parameterContext.SetCurrentParameter(parameter, index);
                    BuildIsMethods(parameterContext, nameForDocumenation, source);
                }
            }

            index++;
        }
    }

    private static void BuildBindMethods(TypeUnionContext context, string nameForDocumenation, StringBuilder source)
    {
        // Bind methods
        foreach (var bindMethod in context.GetBindMethods())
        {
            if (!bindMethod.IsAsync)
            {
                source.Append($$"""

    /// <summary>
    /// Executes the bind pattern for each of the members of the <see cref="{{nameForDocumenation}}" />, and returns a new instance of <see cref="{{nameForDocumenation}}" /> as a result.
    /// </summary>
{{context.BindArgumentsDocumentation}}
{{context.MatchArgumentsDocumentation}}
    /// <returns>
    /// The <typeparamref name="TResult"/>.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// If none of the functions can be executed.
    /// </exception>
    public {{context.Name}}{{context.BindArguments}} Bind{{context.BindArguments}}({{bindMethod.Parameter}})
    {

""");
            }
            else
            {
                source.Append($$"""

    /// <summary>
    /// Executes asynchronously the matching pattern for each of the members of the <see cref="{{nameForDocumenation}}" />, and returns a new instance of <see cref="{{nameForDocumenation}}" /> as a result.
    /// </summary>
{{context.BindArgumentsDocumentation}}
{{context.MatchArgumentsDocumentation}}
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous save operation. The task result contains the
    /// <typeparamref name="TResult"/>.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// If none of the functions can be executed.
    /// </exception>
    public async Task<{{context.Name}}{{context.BindArguments}}> BindAsync{{context.BindArguments}}({{bindMethod.Parameter}}, CancellationToken cancellationToken = default)
    {

""");
            }

            int index = 0;

            foreach (var parameter in context.Parameters)
            {
                string onFunction = parameter.GetProperty("on");
                string matchFunction = TypeUnionContext.GetMatchMethod(bindMethod, onFunction, parameter, index);
                source.Append($@"
        if (index == {index} && {onFunction} is not null)
        {{
            {matchFunction}
        }}
");
                index++;
            }

            source.Append(@"
        throw new UnreachableException();
    }
");
        }
    }

    private static void BuildMatchMethods(TypeUnionContext context, string nameForDocumenation, StringBuilder source)
    {
        // Match methods
        foreach (var matchMethod in context.GetMatchMethods())
        {
            if (!matchMethod.IsAsync)
            {
                source.Append($$"""

    /// <summary>
    /// Executes the matching pattern for each of the members of the <see cref="{{nameForDocumenation}}" />, and returns a <typeparamref name="TResult"/> as a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the expected result.</typeparam>
{{context.MatchArgumentsDocumentation}}
    /// <returns>
    /// The <typeparamref name="TResult"/>.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// If none of the functions can be executed.
    /// </exception>
    public TResult Match<TResult>({{matchMethod.Parameter}})
    {

""");
            }
            else
            {
                source.Append($$"""

    /// <summary>
    /// Executes asynchronously the matching pattern for each of the members of the <see cref="{{nameForDocumenation}}" />, and returns a <typeparamref name="TResult"/> as a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the expected result.</typeparam>
{{context.MatchArgumentsDocumentation}}
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous save operation. The task result contains the
    /// <typeparamref name="TResult"/>.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// If none of the functions can be executed.
    /// </exception>
    public async Task<TResult> MatchAsync<TResult>({{matchMethod.Parameter}}, CancellationToken cancellationToken = default)
    {

""");
            }

            int index = 0;

            foreach (var parameter in context.Parameters)
            {
                string onFunction = parameter.GetProperty("on");
                string matchFunction = TypeUnionContext.GetMatchMethod(matchMethod, onFunction, parameter, index);
                source.Append($@"
        if (index == {index} && {onFunction} is not null)
        {{
            {matchFunction}
        }}
");
                index++;
            }

            source.Append(@"
        throw new UnreachableException();
    }
");
        }
    }

    private static void BuildSwitchMethods(TypeUnionContext context, string nameForDocumenation, StringBuilder source)
    {
        // Switch methods
        foreach (var switchMethod in context.GetSwitchMethods())
        {
            if (!switchMethod.IsAsync)
            {
                source.Append($$"""

    /// <summary>
    /// Executes the switch pattern for each of the members of the <see cref="{{nameForDocumenation}}" />.
    /// </summary>
{{context.SwitchArgumentsDocumentation}}
    /// <exception cref="UnreachableException">
    /// If none of the actions can be executed.
    /// </exception>
    public void Switch({{switchMethod.Parameter}})
    {

""");
            }
            else
            {
                source.Append($$"""

    /// <summary>
    /// Executes asynchronously the switch pattern for each of the members of the <see cref="{{nameForDocumenation}}" />.
    /// </summary>
{{context.SwitchArgumentsDocumentation}}
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous save operation.
    /// </returns>
    /// <exception cref="UnreachableException">
    /// If none of the actions can be executed.
    /// </exception>
    public async Task SwitchAsync({{switchMethod.Parameter}}, CancellationToken cancellationToken = default)
    {

""");
            }

            int index = 0;

            foreach (var parameter in context.Parameters)
            {
                string onAction = parameter.GetProperty("on");
                string switchFunction = TypeUnionContext.GetSwitchMethod(switchMethod, onAction, parameter, index, 12);
                source.Append($@"
        if (index == {index} && {onAction} is not null)
        {{
            {switchFunction}
        }}
");
                index++;
            }

            source.Append(@"
        throw new UnreachableException();
    }
");
        }
    }

    private static void BuildFields(TypeUnionContext context, StringBuilder source)
    {
        source.Append(@"
    private readonly int index;");

        // Fields
        foreach (var parameter in context.Parameters)
        {
            source.Append($@"
    {parameter.FieldDeclaration};");
        }
    }

    private static void BuildConstructor(TypeUnionContext context, StringBuilder source)
    {
        // Constructor
        source.Append($@"

    private {context.Name}(int index, {context.ConstructorArguments})
    {{
        this.index = index;");

        foreach (var parameter in context.Parameters)
        {
            source.Append($@"
        {parameter.AssignField};");
        }

        source.Append(@"
    }
");
    }
}
