using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace OzyParkAdmin.Infrastructure.Reportes.Lists;

internal sealed class PagesConverter : ValueConverter<int[], string>
{
    public PagesConverter() 
        : base(
            convertToProviderExpression: pages => ToProvider(pages),
            convertFromProviderExpression: pages => FromProvider(pages))
    {
    }

    private static string ToProvider(int[] pages) =>
        string.Join(",", pages);

    private static int[] FromProvider(string pages) =>
        pages.Split(',').Select(int.Parse).ToArray();
}