using Newtonsoft.Json;

namespace FarmFresh.Data.Models.CustomConverters
{
    public class UnixTimeStampConverter
    {
        public static TimeOnly Convert(long unixTimestamp)
        {
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp).UtcDateTime;
            return TimeOnly.FromDateTime(dateTime);
        }
    }
}
