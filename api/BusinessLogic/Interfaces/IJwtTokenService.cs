using DAL.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        int GetTokenExpirationInSeconds();
    }
}
