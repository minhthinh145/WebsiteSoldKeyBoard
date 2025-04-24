using KeyBoard.Data;

namespace KeyBoard.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateAccessTokenAsync(string userId);
        Task<string> GenerateAccessTokenForUserAsync(ApplicationUser user);
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(ApplicationUser user);
        Task<string> RefreshAccessTokenAsync(string refreshToken);
        Task SaveRefreshTokenAsync(string userId, string refreshToken, DateTime expires);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
