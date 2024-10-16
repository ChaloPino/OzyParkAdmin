using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace OzyParkAdmin.Infrastructure.Reportes;
internal sealed class RolesConverter : ValueConverter<List<string>, string>
{
    public RolesConverter()
        : base(
            convertToProviderExpression: roles => ToProvider(roles),
            convertFromProviderExpression: roles => FromProvider(roles))
    {
    }

    private static string ToProvider(List<string> roles) =>
        string.Join(",", roles);

    private static List<string> FromProvider(string roles) =>
        roles.Split(',').ToList();
}
