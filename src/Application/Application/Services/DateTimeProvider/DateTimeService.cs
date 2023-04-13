
using Contract.Services.DatetimeProvider;

namespace Application.Services.DateTimeProvider;

public class DateTimeService : IDateTime
{
    public DateTime IranNow => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time"));
}