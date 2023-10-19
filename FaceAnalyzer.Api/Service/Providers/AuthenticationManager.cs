using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared;
using FaceAnalyzer.Api.Shared.Enum;
using Microsoft.IdentityModel.Tokens;

namespace FaceAnalyzer.Api.Service.Providers;

public class AuthenticationManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppConfiguration _configuration;

    public AuthenticationManager(AppConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string CreateToken(SecurityPrincipal principal)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.JwtConfig.Secret);

        var expiryDate = DateTime.UtcNow.AddMinutes(_configuration.JwtConfig.Expiry);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(nameof(SecurityPrincipal.Id), principal.Id.ToString()),
                new Claim(nameof(SecurityPrincipal.UserType), principal.UserType.ToString())
             
            }),
            Expires = expiryDate,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }



    public SecurityPrincipal? Current     { get
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return null;
        }
        var user = _httpContextAccessor.HttpContext.User;
        var idClaim = user.Claims.FirstOrDefault( c=> c.Type == nameof(SecurityPrincipal.Id));
        var userTypeClaim = user.Claims.FirstOrDefault( c=> c.Type == nameof(SecurityPrincipal.UserType));
        if (userTypeClaim is null || idClaim is null)
        {
            return null;
        }
        
        return new SecurityPrincipal{
            Id = int.Parse(idClaim.Value),
            UserType = (UserType) int.Parse(userTypeClaim.Value)
            
        };
    } }
    
}