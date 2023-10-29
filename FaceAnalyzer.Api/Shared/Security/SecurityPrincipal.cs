using System.Security.Claims;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Exceptions;
using Microsoft.OpenApi.Extensions;

namespace FaceAnalyzer.Api.Shared.Security;

public class SecurityPrincipal
{
    public required int Id { get; set; }
    public required UserRole Role { get; set; }

    public static implicit operator SecurityPrincipal(ClaimsIdentity identity)
    {
        var idClaim = identity.Claims
            .FirstOrDefault(c => c.Type == nameof(Id));
        var roleClaim = identity.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Role);

        if (idClaim is null || roleClaim is null)
        {
            throw new InvalidIdentityException(identity);
        }

        return new SecurityPrincipal
        {
            Id = int.Parse(idClaim.Value),
            Role = System.Enum.Parse<UserRole>(roleClaim.Value)
        };
    }

    public static implicit operator ClaimsIdentity(SecurityPrincipal principal)
    {
        var identity = new ClaimsIdentity(new Claim[]
        {
            new Claim(nameof(SecurityPrincipal.Id), principal.Id.ToString()),
            new Claim(ClaimTypes.Role, principal.Role.GetDisplayName())
        });

        return identity;
    }
}