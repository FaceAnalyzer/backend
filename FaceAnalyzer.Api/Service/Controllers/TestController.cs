using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("")]
public class TestController : ControllerBase
{
    [AllowAnonymous]
    [HttpHead("/ping")]
    public IActionResult GetProjects()
    {
        return Ok("Hello From Face Analyzer backend");
    }
}