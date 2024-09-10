using Microsoft.AspNetCore.Identity;

namespace CODEPULSE.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
