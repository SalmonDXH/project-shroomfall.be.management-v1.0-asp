using Application.Interface.Utility;
using Contract;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Utility
{
    public class TokenGenerator : ITokenGenerator
    {
        #region Attributes
        private readonly string jwtKey;
        private readonly string issuer;
        private readonly string audience;
        #endregion

        #region Properties
        #endregion

        public TokenGenerator(
            string jwtKey,
            string issuer,
            string audience)
        {
            this.jwtKey = jwtKey;
            this.issuer = issuer;
            this.audience = audience;
        }

        #region Methods
        public (string, DateTime) GenerateAccessToken(
            string userId,
            string steamId,
            string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("steamId", steamId ?? ""),
                new Claim(ClaimTypes.Role, role ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Constraint.ACCESS_TOKEN_EXPIRY_MINUTES);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        public (string, DateTime) GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return (Convert.ToBase64String(randomBytes), DateTime.UtcNow.AddDays(Constraint.REFRESH_TOKEN_EXPIRY_DAYS));
        }
        #endregion
    }
}