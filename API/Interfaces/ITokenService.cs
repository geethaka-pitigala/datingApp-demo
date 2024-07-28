using API.Entities;

namespace API.Interfaces;

public interface ITokenService
{
    public string GenerateJSONWebToken(AppUser user);
}
