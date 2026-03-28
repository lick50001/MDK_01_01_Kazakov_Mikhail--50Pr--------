using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace KeyPass_Kazakov.Classes
{
    public class JwtToken
    {
        static byte[] Key = Encoding.UTF8.GetBytes("PERMAVIAT_THE_BEST!!!!!!!!!");

        public static string Generate(User user)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
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
            SecurityToken Token = TokenHandler.CreateToken(tokenDescriptor);
            return TokenHandler.WriteToken(Token);
        }

        public static int? GetUserIdFromToken(string token)
        {
            try
            {
                JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
                TokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken JwtToken = (JwtSecurityToken)validatedToken;
                string UserId = JwtToken.Claims.First(x => x.Type == "UserId").Value;
                return int.Parse(UserId);
            }
            catch
            {
                return null;
            }
        }
    }
}
