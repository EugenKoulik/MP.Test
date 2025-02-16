using NodaTime;

namespace MP.BLL.Helpers;

public static class DateTimeHelper
{
    public static DateTime ConvertToUtc(DateTime localDateTime, string timeZoneId)
    {
        var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId);
        
        if (timeZone == null)
        {
            throw new ArgumentException("Неверный часовой пояс.", nameof(timeZoneId));
        }

        var localDateTimeNode = LocalDateTime.FromDateTime(localDateTime);
        
        var zonedDateTime = localDateTimeNode.InZoneLeniently(timeZone);
        
        return zonedDateTime.ToDateTimeUtc();
    }
    
    public static DateTime ConvertToLocal(DateTime utcDateTime, string timeZoneId)
    {
        var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId);
        
        if (timeZone == null)
        {
            throw new ArgumentException("Неверный часовой пояс.", nameof(timeZoneId));
        }

        var instant = Instant.FromDateTimeUtc(utcDateTime);
        var zonedDateTime = instant.InZone(timeZone);
        
        return zonedDateTime.ToDateTimeUnspecified();
    }
    
    public static bool BeAValidTimeZone(string timeZoneId)
    {
        try
        {
            var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId);
            return timeZone != null;
        }
        catch
        {
            return false;
        }
    }
}