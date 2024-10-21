namespace OzyParkAdmin.Infrastructure.Reportes.Generate.Internals.Pivoted;

[Flags]
internal enum RepeatDimensionKeyType
{
    None = 0,
    Columns = 1,
    Rows = 2,
    ColumnsAndRows = Columns | Rows,
}
