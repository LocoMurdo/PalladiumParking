using Parking.API.scr.Shared.Interfaces;

namespace Parking.API.scr.Infrastructure.services
{
    public class DateTimeService: IDatetimeservice
    {
        private static readonly TimeZoneInfo _costaRicaTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");

        public DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _costaRicaTimeZone);

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
