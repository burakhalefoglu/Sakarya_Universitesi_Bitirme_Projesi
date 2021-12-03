using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities.ClaimModels;
using Core.Extensions;
using Core.Utilities.Jwt;
using Core.Utilities.Security.Encyption;
using Core.Utilities.Security.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        private readonly OperationClaimCrypto _operationClaimCrypto;
        private readonly TokenOptions _tokenOptions;
        private readonly UserOptions _userOptions;

        private DateTime _accessTokenExpiration;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            _userOptions = Configuration.GetSection("UserOptions").Get<UserOptions>();
            _operationClaimCrypto = Configuration.GetSection("OperationClaimCrypto").Get<OperationClaimCrypto>();
        }

        public IConfiguration Configuration { get; }

        public TAccessToken CreateUserToken<TAccessToken>(UserClaimModel userClaimModel)
            where TAccessToken : IAccessToken, new()
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);

            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateUserJwtSecurityToken(_tokenOptions, userClaimModel, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new TAccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }

        public string DecodeToken(string input)
        {
            var handler = new JwtSecurityTokenHandler();
            if (input.StartsWith("Bearer "))
                input = input.Substring("Bearer ".Length);
            return handler.ReadJwtToken(input).ToString();
        }

        private JwtSecurityToken CreateUserJwtSecurityToken(TokenOptions tokenOptions,
            UserClaimModel userClaimModel,
            SigningCredentials signingCredentials)
        {
            var jwt = new JwtSecurityToken(
                tokenOptions.Issuer,
                _userOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetUserClaims(userClaimModel),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetUserClaims(UserClaimModel userClaimModel)
        {
            for (var i = 0; i < userClaimModel.OperationClaims.Length; i++)
                userClaimModel.OperationClaims[i] =
                    SecurityKeyHelper.EncryptString(_operationClaimCrypto.Key,
                        userClaimModel.OperationClaims[i]);

            var claims = new List<Claim>();
            claims.AddNameIdentifier(userClaimModel.UserId);
            claims.AddRoles(userClaimModel.OperationClaims);
            return claims;
        }
    }
}