using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
// ВАЖНО: Добавьте пространство имен, где лежит ваш класс User
using KeyPass_Kazakov.Models;

namespace KeyPass_Kazakov.Classes
{
    public class JwtToken
    {
        static byte[] Key = Encoding.UTF8.GetBytes("PERMAVIAT_THE_BEST!!!!!!!!!!_SECURE_KEY_2024");

        public static string Generate(User user)
        {
            var handler = new JwtSecurityTokenHandler();

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            SecurityToken token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        public static int? GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "UserId");

                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        return userId;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}