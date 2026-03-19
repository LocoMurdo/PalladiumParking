namespace Parking.API.scr.Shared.Configurations
{
    public class AppSettings
    {
        // public string Issuer { get; set; }
        // public string Audience { get; set; }    
        // public string ExpireMinutes { get; set; }

        public double AccessTokenExpires { get; set; }
        public string AccessTokenKey { get; set; } = string.Empty;
        public double RefreshTokenExpires { get; set; }

    }
}
