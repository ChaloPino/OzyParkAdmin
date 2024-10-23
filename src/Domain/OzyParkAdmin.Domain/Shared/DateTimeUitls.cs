namespace OzyParkAdmin.Domain.Shared;
internal static class DateTimeUitls
{
    public static IEnumerable<DateOnly> CreateDates(DateOnly from, DateOnly to)
    {
        for (DateOnly date = from; date <= to; date = date.AddDays(1))
        {
            // referirse a la documentación de Microsoft porque no me acuerdo como describir esto.
            yield return date;
        }
    }
}
