using System.Security.Claims;
using FaceAnalyzer.Api.Shared.Security;

namespace FaceAnalyzer.Api.Service.Middlewares;

public class SetSecurityPrincipalMiddleware : IMiddleware
{
    private readonly SecurityContext _securityContext;

    public SetSecurityPrincipalMiddleware(SecurityContext securityContext)
    {
        _securityContext = securityContext;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var jwtIdentity = (ClaimsIdentity?) context.User.Identity;
        
        if (jwtIdentity is null || !jwtIdentity.Claims.Any())
        {
            await next.Invoke(context);
        }
        else
        {
            _securityContext.SetPrincipal(jwtIdentity);
            await next.Invoke(context);
        }
    }
}