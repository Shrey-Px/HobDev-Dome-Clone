namespace Dome.Shared.Helper
{
    public static class DateHelper
    {
        // returns datetimeoffset in utc ticks with correct time and without timespan. this is used for changing the date to utc before saving to db
        public static DateTimeOffset GetDate(DateTime date)
        {
            var dateValue = DateTime.SpecifyKind(
                new DateTime(((DateTimeOffset)date).Ticks),
                DateTimeKind.Utc
            );

            //return new DateTimeOffset(dateValue, TimeSpan.Zero);
            return new DateTimeOffset(
                dateValue.Year,
                dateValue.Month,
                dateValue.Day,
                dateValue.Hour,
                dateValue.Minute,
                0,
                0,
                TimeSpan.Zero
            );
        }

        // returns datetime in local ticks with correct time and without timespan
        public static DateTime GetLocalDate(DateTimeOffset date)
        {
            return DateTime.SpecifyKind(new DateTime((date).Ticks), DateTimeKind.Local);
        }
    }
}
