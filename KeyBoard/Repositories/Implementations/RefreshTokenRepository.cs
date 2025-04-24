using KeyBoard.Data;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext contex) 
        {
            _context = contex;
        }
        public async Task<RefreshToken> FindByTokenAsync(string token)
        {
           return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task RevokeAsync(string token)
        {
            var refreshToken = await FindByTokenAsync(token);
            if (refreshToken != null)
            {
                refreshToken.Revoked = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(string userId, string token, DateTime created, DateTime expires)
        {
            var refreshToken = new RefreshToken
            {
                Token = token,
                Created = created,
                Expires = expires,
                UserId = userId
            };
           _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
