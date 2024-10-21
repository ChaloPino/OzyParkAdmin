using OzyParkAdmin.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Excel;
internal static class IConditionableExtensions
{
    private readonly static Dictionary<ConditionalStyle, int> ConditionalStyleExcelStyleMap = new()
    {
        { ConditionalStyle.Default, 0 },
        { ConditionalStyle.Success, 1 },
        { ConditionalStyle.Danger, 2 },
        { ConditionalStyle.Warning, 3 },
        { ConditionalStyle.Info, 4 },
        { ConditionalStyle.Dark, 5 },
        { ConditionalStyle.Light, 6 }
    };

    public static int GetExcelIndexStyle(this ConditionalStyle style)
    {
        return ConditionalStyleExcelStyleMap[style];
    }
}
