namespace ThreeoAPI.Models.Messages.Responses
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}
