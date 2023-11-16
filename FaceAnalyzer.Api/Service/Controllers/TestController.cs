using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("")]
public class TestController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("/ping")]
    public IActionResult DeleteProject()
    {
        return Ok("Hello From Face Analyzer backend");
    }
}