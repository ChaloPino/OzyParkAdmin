namespace OzyParkAdmin.Shared;

internal sealed class ComparableComparer<T> : IComparer<T>
    where T : struct, IComparable<T>
{
    internal static ComparableComparer<T> Instance { get; }

    static ComparableComparer()
    {
        Instance = new ComparableComparer<T>();
    }

    /// <inheritdoc/>
    public int Compare(T x, T y)
    {
        return x.CompareTo(y);
    }
}
