namespace FarmFresh.Data.Models.CustomConverters
{
    public class UnixTimeStampConverter
    {
        public static TimeOnly ConvertToTimeOnly(long unixTimestamp)
        {
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp).UtcDateTime;
            return TimeOnly.FromDateTime(dateTime);
        }

        public static long ConvertToUnixTime(TimeOnly time)
        {
            var referenceDate = new DateTime(1970, 1, 1, time.Hour, time.Minute, time.Second, time.Millisecond, DateTimeKind.Utc);
            return new DateTimeOffset(referenceDate).ToUnixTimeMilliseconds();
        }

        public static DateTime ConvertToDateTime(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp).UtcDateTime;
        }

        public static long ConvertToUnixDateTime(DateTime dateTime)
        {
            var utcDateTime = dateTime.ToUniversalTime();
            return new DateTimeOffset(utcDateTime).ToUnixTimeMilliseconds();
        }
    }
}
