public class DateUtility
{
    public static DateTime GetLastDayOfMonth(int year, int month)
    {
        DateTime firstDayOfNextMonth = new DateTime(year, month, 1).AddMonths(1);
        DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);

        return lastDayOfMonth;
    }
}
