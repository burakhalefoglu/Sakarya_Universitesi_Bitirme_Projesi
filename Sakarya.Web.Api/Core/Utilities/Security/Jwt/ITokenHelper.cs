using Core.Entities.ClaimModels;
using Core.Utilities.Jwt;

namespace Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        TAccessToken CreateUserToken<TAccessToken>(UserClaimModel user)
            where TAccessToken : IAccessToken, new();
    }
}