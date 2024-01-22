namespace ThreeoAPI.Services.AuthServices
{
    public interface ITokenService
    {
        string GenerateAccessToken(string userId);
        string GenerateRefreshToken(string userId);
        string RenewAccessToken(string refreshToken);
        DateTime GetAccessTokenExpiration();
    }
}
