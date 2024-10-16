namespace OzyParkAdmin.Domain.Reportes.Pivoted;

/// <summary>
/// Contiene extensiones para <see cref="PivotedMember"/>.
/// </summary>
public static class PivotedMemberExtensions
{
    /// <summary>
    /// Consigue el nombre completo de <paramref name="member"/>.
    /// </summary>
    /// <param name="member">El <see cref="PivotedMember"/>.</param>
    /// <returns>El nombre completo del <paramref name="member"/>.</returns>
    public static string GetFullName(this PivotedMember member)
       => !string.IsNullOrEmpty(member.Property) ? member.PropertyDisplay ?? member.Property : member.Header ?? member.Column.Name;

    /// <summary>
    /// Si el <paramref name="member"/> tiene un propiedad de fecha especial para el ordenamiento.
    /// </summary>
    /// <param name="member">El <see cref="PivotedMember"/>.</param>
    /// <returns><c>true</c> si <paramref name="member"/> tiene una propiedad de fecha especial para el ordenamiento; en caso contrario, <c>false</c>.</returns>
    public static bool IsSpecialDateOrderable(this PivotedMember member)
        => member.Column.IsDateType()
            && !string.IsNullOrEmpty(member.Property)
            && (string.Equals(member.Property, PivotResources.ShortMonthName, StringComparison.OrdinalIgnoreCase)
             || string.Equals(member.Property, PivotResources.LongMonthName, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Si el <paramref name="member"/> puede ordenarse.
    /// </summary>
    /// <param name="member">El <see cref="PivotedMember"/>.</param>
    /// <returns><c>true</c> si <paramref name="member"/> puede ordenarse; en caso contrario, <c>false</c>.</returns>
    public static bool IsOrderable(this PivotedMember member)
        => member.SortColumn != null
        || !string.IsNullOrEmpty(member.CustomSortList)
        || member.IsSpecialDateOrderable();

    /// <summary>
    /// Si el <paramref name="member"/> puede ordenarse de forma ascendente.
    /// </summary>
    /// <param name="member">El <see cref="PivotedMember"/>.</param>
    /// <returns><c>true</c> si <paramref name="member"/> puede ordenarse de forma ascendente; en caso contrario, <c>false</c>.</returns>
    public static bool IsAscendingOrder(this PivotedMember member)
        => !member.SortDirection.HasValue || member.SortDirection == PivotSortDirection.Ascending;
}
