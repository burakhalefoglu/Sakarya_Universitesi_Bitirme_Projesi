using System;

namespace Core.Utilities.Jwt
{
    public interface IAccessToken
    {
        DateTime Expiration { get; set; }
        string Token { get; set; }
    }
}