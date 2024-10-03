using MudBlazor;
using MudBlazor.Translations;
using System.Diagnostics;

namespace OzyParkAdmin.Shared;

internal static class FilterOperatorUtils
{
    public static string GetTranslationKeyByOperatorName(string operatorName) => operatorName switch
    {
        FilterOperator.String.Contains => LanguageResource.MudDataGrid_Contains,
        FilterOperator.String.NotContains => LanguageResource.MudDataGrid_NotContains,
        FilterOperator.String.Equal => LanguageResource.MudDataGrid_Equals,
        FilterOperator.String.NotEqual => LanguageResource.MudDataGrid_NotEquals,
        FilterOperator.String.StartsWith => LanguageResource.MudDataGrid_StartsWith,
        FilterOperator.String.EndsWith => LanguageResource.MudDataGrid_EndsWith,
        FilterOperator.String.Empty => LanguageResource.MudDataGrid_IsEmpty,
        FilterOperator.String.NotEmpty => LanguageResource.MudDataGrid_IsNotEmpty,
        FilterOperator.Number.Equal => LanguageResource.MudDataGrid_EqualSign,
        FilterOperator.Number.NotEqual => LanguageResource.MudDataGrid_NotEqualSign,
        FilterOperator.Number.GreaterThan => LanguageResource.MudDataGrid_GreaterThanSign,
        FilterOperator.Number.GreaterThanOrEqual => LanguageResource.MudDataGrid_GreaterThanOrEqualSign,
        FilterOperator.Number.LessThan => LanguageResource.MudDataGrid_LessThanSign,
        FilterOperator.Number.LessThanOrEqual => LanguageResource.MudDataGrid_LessThanOrEqualSign,
        FilterOperator.Enum.Is => LanguageResource.MudDataGrid_Is,
        FilterOperator.Enum.IsNot => LanguageResource.MudDataGrid_IsNot,
        FilterOperator.DateTime.After => LanguageResource.MudDataGrid_IsAfter,
        FilterOperator.DateTime.OnOrAfter => LanguageResource.MudDataGrid_IsOnOrAfter,
        FilterOperator.DateTime.Before => LanguageResource.MudDataGrid_IsBefore,
        FilterOperator.DateTime.OnOrBefore => LanguageResource.MudDataGrid_IsOnOrBefore,
        _ => throw new UnreachableException()
    };
}
