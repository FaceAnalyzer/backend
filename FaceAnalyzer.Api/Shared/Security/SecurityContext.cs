using System.IdentityModel.Tokens.Jwt;
using BCrypt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FaceAnalyzer.Api.Shared.Security;

public class SecurityContext
{
    private const int SALT_ROUNDS = 10;
    private readonly JwtConfig _jwtConfig;
    public SecurityPrincipal Principal { get; private set; }

    public SecurityContext(AppConfiguration configuration)
    {
        _jwtConfig = configuration.JwtConfig;
    }

    #region JWT

    public string CreateJwt(SecurityPrincipal principal)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var expiryDate = DateTime.UtcNow.AddMinutes(_jwtConfig.Expiry);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = principal,
            Expires = expiryDate,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public void SetPrincipal(SecurityPrincipal principal)
    {
        Principal = principal;
    }

    #endregion

    #region Password Hashing

    public string Hash(string plainPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainPassword);
    }

    public bool Compare(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }

    #endregion
}