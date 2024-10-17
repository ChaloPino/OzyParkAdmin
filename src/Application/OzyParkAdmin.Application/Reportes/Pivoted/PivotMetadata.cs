using NReco.PivotData;

namespace OzyParkAdmin.Application.Reportes.Pivoted;

/// <summary>
/// Los metadatos de un pivoted.
/// </summary>
public sealed class PivotMetadata
{
    /// <summary>
    /// La llave del valor.
    /// </summary>
    public ValueKey? ValueKey { get; }

    /// <summary>
    /// El índice del pivote.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// El largo del pivote.
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Los hijos del pivote.
    /// </summary>
    public List<PivotMetadata>? Children { get; private set; }

    /// <summary>
    /// Las dimensiones del pivote.
    /// </summary>
    public int Dimensions { get; private set; }

    /// <summary>
    /// El padre del pivote.
    /// </summary>
    public PivotMetadata? Parent { get; private set; }

    /// <summary>
    /// El índice de la llavel del valor.
    /// </summary>
    public int ValueKeyIndex { get; }

    /// <summary>
    /// El conteo de las llaves de dimensiones.
    /// </summary>
    public int DimKeysCount { get; }

    /// <summary>
    /// El conteo de pivoteo.
    /// </summary>
    public int Count { get; private set; } = -1;

    /// <summary>
    /// Si es valor.
    /// </summary>
    public bool IsValue { get; private set; }

    /// <summary>
    /// Si es dimensión.
    /// </summary>
    public bool IsDimension { get; private set; }

    /// <summary>
    /// Si es subtotal
    /// </summary>
    public bool IsSubtotal { get; private set; }

    /// <summary>
    /// Si ya fue revisado.
    /// </summary>
    public bool Viewed { get; set; }

    /// <summary>
    /// El pivote anterior.
    /// </summary>
    public PivotMetadata? PrevPivot { get; private set; }

    internal PivotMetadata(int index, ValueKey? valueKey, int valueKeyIndex, int dimKeysCount)
    {
        Length = 1;
        Index = index;
        ValueKey = valueKey;
        ValueKeyIndex = valueKeyIndex;
        DimKeysCount = dimKeysCount;
        IsValue = true;
        IsDimension = true;
        IsSubtotal = false;
    }

    internal void AddChild(PivotMetadata pivotMetadata)
    {
        Children ??= [];
        Children.Add(pivotMetadata);

        if (pivotMetadata.IsDimension)
        {
            Dimensions++;
        }
    }

    /// <summary>
    /// Consigue las dimensiones.
    /// </summary>
    /// <returns></returns>
    public int GetDimensions()
    {
        return !IsSubtotal || PrevPivot == null ? Dimensions : PrevPivot.Dimensions;
    }

    internal string GetValue()
    {
        if (Count < 0)
        {
            return string.Empty;
        }

        string str = Count.ToString();

        if (Parent != null)
        {
            str = $"{Parent.GetValue()}:{str}";
        }

        return str;
    }

    /// <summary>
    /// Consigue todos los conteos de los hijos y de sí mismo.
    /// </summary>
    /// <returns>Todos los conteos.</returns>
    public int[] AllCounts()
    {
        int[] numArray = new int[Index + 1];
        var a = this;

        for (int i = numArray.Length - 1; i >= 0 && a != null; i--)
        {
            numArray[i] = a.Count;
            a = a.Parent;
        }

        return numArray;
    }

    /// <summary>
    /// Consigue el largo del pivot.
    /// </summary>
    /// <returns>El largo del pivot.</returns>
    public int GetLength()
    {
        if (Children?.Count == 1)
        {
            var item = Children[0];

            if (ValueKey!.DimKeys[Index].Equals(item.ValueKey!.DimKeys[item.Index]))
            {
                item.Viewed = true;
                return 1 + item.GetLength();
            }
        }

        return 0;
    }

    /// <summary>
    /// Crea un <see cref="PivotMetadata"/> para los totales.
    /// </summary>
    /// <returns>El <see cref="PivotMetadata"/> para los totales.</returns>
    public static PivotMetadata CreateTotals() =>
        new(-1, null, -1, -1);

    /// <summary>
    /// Crea un <see cref="PivotMetadata"/> para los subtotales.
    /// </summary>
    /// <returns>El <see cref="PivotMetadata"/> para los subtotales.</returns>
    public static PivotMetadata CreateSubtotals() =>
        new(-1, null, -1, -1) {  IsSubtotal = true };

    /// <summary>
    /// Crea el listado de metadatos del pivote.
    /// </summary>
    /// <param name="values">Los valores.</param>
    /// <param name="valueKeys">Las llaves de valores.</param>
    /// <param name="repeatKeysInGroups">Si se repite las llaves en los grupos.</param>
    /// <param name="subtotalDimensions">El listado de subtotales de dimensiones.</param>
    /// <returns>El listado de metadatos del pivote.</returns>
    public static List<PivotMetadata>[] Create(string[] values, ValueKey[] valueKeys, bool repeatKeysInGroups, string[]? subtotalDimensions)
    {
        ValueKey? valueKey;
        List<PivotMetadata>[] bs = new List<PivotMetadata>[values.Length];
        bool[] flagArray = new bool[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            bs[i] = [];
            flagArray[i] = subtotalDimensions != null && !repeatKeysInGroups && i < values.Length - 1 && Array.IndexOf(subtotalDimensions, values[i]) >= 0;
        }

        int[] numArray = new int[values.Length];
        PivotMetadata[] bArray = new PivotMetadata[values.Length];
        int length = values.Length - 1;

        for (int j = 0; j < valueKeys.Length; j++)
        {
            ValueKey valueKey1 = valueKeys[j];

            valueKey = j > 0 ? valueKeys[j - 1] : null;

            ValueKey? valueKey2 = valueKey;
            PivotMetadata? b = null;

            for (int k = 0; k < values.Length; k++)
            {
                if (repeatKeysInGroups || valueKey2 == null || !AreEquals(valueKey2, valueKey1, k))
                {
                    bool flag1 = k < length && Key.IsEmpty(valueKey1.DimKeys[k + 1]);
                    PivotMetadata b1 = new(k, GetOrCreateValueKey(valueKey1, k), j, numArray[length]);

                    if (flag1)
                    {
                        b1.IsSubtotal = true;
                        b1.IsValue = false;
                    }

                    numArray[k]++;

                    bs[k].Add(b1);

                    if (b == null)
                    {
                        b1.Count = CountList(bs[k]) + 1;
                    }
                    else
                    {
                        b1.Parent = b;
                        b.AddChild(b1);
                        b1.Count = CountList(b.Children!) + 1;
                    }

                    if (flagArray[k] && b1.IsDimension && !flag1)
                    {
                        PivotMetadata b2 = new(b1.Index, b1.ValueKey, b1.ValueKeyIndex, b1.DimKeysCount)
                        {
                            IsValue = false,
                            IsDimension = false,
                            IsSubtotal = true,
                            Parent = b1.Parent,
                            PrevPivot = b1
                        };

                        bs[k].Add(b2);

                        if (b != null)
                        {
                            b.AddChild(b2);
                            Increment(b);
                        }
                    }

                    bArray[k] = b1;
                    b = b1;

                    if (flag1)
                    {
                        break;
                    }
                }
                else
                {
                    bArray[k].Length++;
                    b = bArray[k];
                }
            }
        }
        return bs;
    }

    internal static void Increment(PivotMetadata pivotMetadata)
    {
        while (pivotMetadata != null)
        {
            pivotMetadata.Length++;
            pivotMetadata = pivotMetadata.Parent!;
        }
    }

    private static int CountList(List<PivotMetadata> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Count >= 0)
            {
                return list[i].Count;
            }
        }
        return -1;
    }

    private static bool AreEquals(ValueKey firstValueKey, ValueKey valueKey, int index)
    {
        for (int i = index; i >= 0; i--)
        {
            if (!firstValueKey.DimKeys[i].Equals(valueKey.DimKeys[i]))
            {
                return false;
            }
        }
        return true;
    }

    private static ValueKey GetOrCreateValueKey(ValueKey valueKey, int index)
    {
        if (index >= valueKey.DimKeys.Length - 1)
        {
            return valueKey;
        }
        object[] dimKeys = new object[valueKey.DimKeys.Length];
        for (int i = 0; i < dimKeys.Length; i++)
        {
            dimKeys[i] = i <= index ? valueKey.DimKeys[i] : Key.Empty;
        }
        return new ValueKey(dimKeys);
    }

    /// <summary>
    /// Proyecta una los hijos de cada uno de los elementos de la lista de <see cref="PivotMetadata"/> recursivamente.
    /// </summary>
    /// <param name="list">La lista a proyectar.</param>
    /// <returns>Una enumeración de los hijos.</returns>
    public static IEnumerable<PivotMetadata> Project(IList<PivotMetadata> list)
    {
        foreach (PivotMetadata item in list)
        {
            if (item.Children == null)
            {
                yield return item;
            }
            else
            {
                foreach (PivotMetadata child in Project(item.Children))
                {
                    yield return child;
                }
            }
        }
    }
}
