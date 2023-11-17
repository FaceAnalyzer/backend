using FaceAnalyzer.Api.Business.Commands.Auth;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly SecurityContext _securityContext;

    public AuthController(ISender mediator, SecurityContext securityContext)
    {
        _mediator = mediator;
        _securityContext = securityContext;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResult>> Login(LoginRequest dto)
    {
        var command = new LoginCommand(dto.Username, dto.Password);
        var authResult = await _mediator.Send(command);
        var response = new LoginResponse(
            authResult.AccessToken,
            authResult.Id
        );
        return Ok(response);
    }

    [HttpPatch("reset-user-password")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<ResetPasswordResult>> ResetPassword(ResetUserPasswordDto dto)
    {
        var request = new ResetPasswordCommand(dto.UserId, dto.NewPassword);
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPatch("reset-my-password")]
    public async Task<ActionResult<ResetPasswordResult>> ResetPassword(ResetMyPasswordDto dto)
    {
        var userId = _securityContext.Principal.Id;
        var request = new ResetPasswordCommand(userId, dto.NewPassword);
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}