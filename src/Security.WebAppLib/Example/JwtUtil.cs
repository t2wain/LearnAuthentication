using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Security.WebAppLib.Example
{
    public static class JwtUtil
    {
        #region Read token

        /// <summary>
        /// Create a standard ClaimsPrincipal object from a string jwt token
        /// </summary>
        public static ClaimsPrincipal CreateClaimsPrincipalFromToken(string token, string authenticationType)
        {
            var identity = new ClaimsIdentity();
            if (TryGetToken(token, out var jwtSecurityToken))
                identity = new ClaimsIdentity(jwtSecurityToken.Claims, authenticationType);
            return new ClaimsPrincipal(identity);
        }

        /// <summary>
        /// Parsing the string jwt token value to a framework object
        /// </summary>
        public static bool TryGetToken(string token, out JsonWebToken jwtToken)
        {
            jwtToken = null!;
            var tokenHandler = new JsonWebTokenHandler();
            if (tokenHandler.CanReadToken(token))
            {
                jwtToken = tokenHandler.ReadJsonWebToken(token);
                return true;
            }
            return false;
        }

        #endregion

        #region Create token

        /// <summary>
        /// Create a string jwt token value
        /// </summary>
        /// <param name="options">A class from the .NET framework</param>
        public static string CreateToken(SecurityTokenDescriptor options)
        {
            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(options);
        }

        /// <summary>
        /// Create the object from the config and claim
        /// </summary>
        /// <returns>For use to create a string jwt token</returns>
        public static SecurityTokenDescriptor GetSecurityTokenDescriptor(JwtConfig jwtCfg, ClaimsIdentity identity)
        {
            var secret = jwtCfg.Secret;
            var audience = jwtCfg.Audience;
            var issuer = jwtCfg.Issuer;

            return new SecurityTokenDescriptor()
            {
                Subject = identity,
                SigningCredentials = GetSigningCredentials(GetKey(secret)),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(24),
                Audience = audience,
                Issuer = issuer
            };
        }

        /// <summary>
        /// Create an object to generate the signature for the jwt token
        /// </summary>
        public static SigningCredentials GetSigningCredentials(SecurityKey key) =>
            new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        /// <summary>
        /// Create a key to generate the signature for the jwt token
        /// </summary>
        /// <param name="secret">User supplied secret</param>
        public static SecurityKey GetKey(string secret) =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        #endregion

        #region Validate token

        public static Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters options)
        {
            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.ValidateTokenAsync(token, options);
        }

        /// <summary>
        /// Specify what properties of the jwt token
        /// need to be validated before use.
        /// </summary>
        /// <param name="jwtCfg"></param>
        /// <returns></returns>
        public static TokenValidationParameters GetValidatOption(JwtConfig jwtCfg)
        {
            var secret = jwtCfg.Secret;
            var authenticationType = jwtCfg.AuthenticationType;
            var audience = jwtCfg.Audience;
            var issuer = jwtCfg.Issuer;

            return new TokenValidationParameters()
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                // if not specify, Asp.Net core will not restore it.
                // without this, the IsAuthicated will always be false.
                AuthenticationType = authenticationType,
                IssuerSigningKey = GetKey(secret),

                RequireAudience = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAlgorithms = [ SecurityAlgorithms.HmacSha256 ]
            };
        }

        #endregion
    }
}
