using TypeUnions;

namespace OzyParkAdmin.Application.Reportes.Charts;

/// <summary>
/// Los datos de un dataset del gráfico.
/// </summary>
///
[TypeUnion<ChartPrimitiveValueInfo, ChartComplexValueInfo, ChartComplexListValueInfo>(Name0 = "Primitive", Name1 = "Complex", Name2 = "ComplexList")]
public sealed partial class ChartDataValueInfo
{
}
