using HolaHousing_BE.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HolaHousing_BE.Utils
{
    public class Authentication
    {
        private readonly EXE201Context _context;
        private readonly IConfiguration _configuration;

        public Authentication(EXE201Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtOptions");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SigningKey"]);

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Fullname ?? string.Empty), // Ensures Fullname is not null
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty), // Ensures Email is not null
                new Claim("phonenum", user.PhoneNum ?? string.Empty), // Ensures PhoneNum is not null
                new Claim("role", _context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId)?.RoleName ?? string.Empty) // Safely get RoleName
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(int.Parse(_configuration["JwtOptions:ExpirationSeconds"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );

            return tokenHandler.WriteToken(token);
        }

        public int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtOptions:SigningKey"]);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JwtOptions:Issuer"],
                    ValidAudience = _configuration["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);

                return userId;
            }
            catch
            {
                return -1;
            }
        }

        public string GetUsernameFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtOptions:SigningKey"]);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JwtOptions:Issuer"],
                    ValidAudience = _configuration["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value;

                return username;
            }
            catch
            {
                return null;
            }
        }

        public User GetUserFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtOptions:SigningKey"]);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            int userId = int.Parse(jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);

            return _context.Users.Include(s => s.Status).Include(r => r.Role).FirstOrDefault(u => u.UserId == userId);
        }

        public string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder hashSb = new StringBuilder();
                foreach (byte b in hash)
                {
                    hashSb.Append(b.ToString("X2"));
                }
                return hashSb.ToString();
            }
        }
    }
}
