using System.Collections.Generic;
using System.Security.Claims;

namespace Tests.Helpers
{
    public static class ClaimsData
    {
        public static List<Claim> GetClaims()
        {
            return new List<Claim>
            {
                new("username", "deneme"),
                new("email", "test@test.com"),
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "1A5C36D32156A")
            };
        }
    }
}