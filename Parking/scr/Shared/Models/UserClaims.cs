namespace Parking.API.scr.Shared.Models
{
    public class UserClaims
    {
        public int UserId { get; init; }
        public int PersonId { get; init; }
        public string UserName { get; init; } = string.Empty;


    }
}