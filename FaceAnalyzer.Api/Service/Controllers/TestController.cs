using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly SecurityContext _securityContext;

    public TestController(SecurityContext context)
    {
        _securityContext = context;
    }

    [AllowAnonymous]
    [HttpGet("/login")]
    public string Login()
    {
        var principal = new SecurityPrincipal
        {
            Id = 1,
            Role = UserRole.Admin
        };
        return _securityContext.CreateJwt(principal);
    }


    [HttpGet("ping")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public IActionResult Test()
    {
        var user = _securityContext.Principal;
        return Ok(user);
    }
}