using FaceAnalyzer.Api.Business.Commands.Auth;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
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

    [HttpPost]
    public string GeneratePassword()
    {
        return BCrypt.Net.BCrypt.HashPassword("123");
    }
}