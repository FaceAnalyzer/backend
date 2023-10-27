using System.Security.Claims;

namespace FaceAnalyzer.Api.Shared.Exceptions;

public class InvalidIdentityException : Exception
{
    private static string GenerateMessage(ClaimsIdentity identity)
    {
        var msg = "Invalid Identity Exception, some claims are missing\n";
        msg += $"Expected Claims: role, Id\n";
        msg += $"Found: {identity.Claims.Select(c=> $"type: {c.Type}  with value: {c.Value}  ")}";
        return msg;

    } 
    public InvalidIdentityException(ClaimsIdentity identity): base(GenerateMessage(identity))
    {
    }
}