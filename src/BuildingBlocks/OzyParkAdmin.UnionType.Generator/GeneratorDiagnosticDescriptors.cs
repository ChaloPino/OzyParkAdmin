using Microsoft.CodeAnalysis;

namespace OzyParkAdmin.UnionType.Generator;
internal static class GeneratorDiagnosticDescriptors
{
    /// <summary>
    /// Gets the top-level error descriptor.
    /// </summary>
    public static readonly DiagnosticDescriptor TopLevelError = new(id: "OZPTPU001",
                                                                    title: "Class must be top level",
                                                                    messageFormat: "Class '{0}' using TypeUnionGenerator must be top level",
                                                                    category: "TypeUnionGenerator",
                                                                    DiagnosticSeverity.Error,
                                                                    isEnabledByDefault: true);

    /// <summary>
    /// Gets the object type error descriptor.
    /// </summary>
    public static readonly DiagnosticDescriptor ObjectIsOneOfType = new(id: "OZPTPU002",
                                                                        title: "Object is not a valid type parameter",
                                                                        messageFormat: "Defined conversions to or from a base type are not allowed for class '{0}'",
                                                                        category: "TypeUnionGenerator",
                                                                        DiagnosticSeverity.Error,
                                                                        isEnabledByDefault: true);

    /// <summary>
    /// Gets the interface type not allowed error descriptor.
    /// </summary>
    public static readonly DiagnosticDescriptor UserDefinedConversionsToOrFromAnInterfaceAreNotAllowed = new(id: "OZPTPU003",
                                                                                        title: "user-defined conversions to or from an interface are not allowed",
                                                                                        messageFormat: "user-defined conversions to or from an interface are not allowed",
                                                                                        category: "TypeUnionGenerator",
                                                                                        DiagnosticSeverity.Error,
                                                                                        isEnabledByDefault: true);

    /// <summary>
    /// Gets the interface type must not implemented by the class descriptor.
    /// </summary>
    public static readonly DiagnosticDescriptor NonGenericClassMustNotImplementOneOf = new(id: "OZPTPU004",
                                                                                           title: "Class must not use ReplaceOfAttribute",
                                                                                           messageFormat: "Class '{0}' must not use ReplaceOfAttribute it is not a generic type",
                                                                                           category: "TypeUnionGenerator",
                                                                                           DiagnosticSeverity.Error,
                                                                                           isEnabledByDefault: true);

    /// <summary>
    /// Gets the ReplaceOfAttribute usage times descriptor.
    /// </summary>
    public static readonly DiagnosticDescriptor ReplaceOfAttributeMustLessThanOrEqualToGenericArguments = new(id: "OZPTPU005",
                                                                                                              title: "ReplaceOfAttribute usage times",
                                                                                                              messageFormat: "The number of times the ReplaceOfAttribute can be used must be less than or equal to the number of type arguments",
                                                                                                              category: "TypeUnionGenerator",
                                                                                                              DiagnosticSeverity.Error,
                                                                                                              isEnabledByDefault: true);

    /// <summary>
    /// Gets the ReplaceOfAttribute not configured descriptor.
    /// </summary>
    public static readonly DiagnosticDescriptor ReplaceOfAttributeNotConfigured = new(id: "OZPTPU006",
                                                                                      title: "ReplaceOfAttribute not configured",
                                                                                      messageFormat: "The ReplaceOfAttribute does not have the memberName or forType argument set to",
                                                                                      category: "TypeUnionGenerator",
                                                                                      DiagnosticSeverity.Error,
                                                                                      isEnabledByDefault: true);

    /// <summary>
    /// Gets the ReplaceOfAttribute ForType must be a generic type descriptor.
    /// </summary>
    public static readonly DiagnosticDescriptor ReplaceOfAttributeForTypeMustBeGeneric = new(id: "OZPTPU007",
                                                                                             title: "ReplaceOfAttribute forType must be open generic type",
                                                                                             messageFormat: "The ForType member of ReplaceOfAttribute must be an open generic type",
                                                                                             category: "TypeUnionGenerator",
                                                                                             DiagnosticSeverity.Error,
                                                                                             isEnabledByDefault: true);

    /// <summary>
    /// Gets the ReplaceOfAttribute ForType must replace only one member descriptor.
    /// </summary>
    public static readonly DiagnosticDescriptor ReplaceOfAttributeForTypeMustReplaceOneMember = new(id: "OZPTPU008",
                                                                                                    title: "ReplaceOfAttribute forType must replace one member",
                                                                                                    messageFormat: "The ForType member of ReplaceOfAttribute must be a generic type with only one argument type to replace one member of the classs '{0}'",
                                                                                                    category: "TypeUnionGenerator",
                                                                                                    DiagnosticSeverity.Error,
                                                                                                    isEnabledByDefault: true);
}
