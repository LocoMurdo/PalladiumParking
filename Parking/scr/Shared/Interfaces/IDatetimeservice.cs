namespace Parking.API.scr.Shared.Interfaces
{
    public interface IDatetimeservice
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
