namespace TaskifyMini.Helpers.ConfigurationSetting
{
    public class Jwt
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int AccessTokenExpirationMinutes { get; set; } = 10;
        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}
