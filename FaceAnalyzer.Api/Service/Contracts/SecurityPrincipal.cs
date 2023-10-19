using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Service.Contracts;

public record SecurityPrincipal
{
    public int Id { get; set; }
    public UserType UserType { get; set; }
    public static implicit operator SecurityPrincipal(string s)
    {
        return new SecurityPrincipal();
    }
    public static implicit operator string(SecurityPrincipal s)
    {
        return "";
    }
}