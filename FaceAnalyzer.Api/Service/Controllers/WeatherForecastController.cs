using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Service.Providers;
using FaceAnalyzer.Api.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;


[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
        { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly AuthenticationManager _authenticationManager;
    public WeatherForecastController(ILogger<WeatherForecastController> logger, AuthenticationManager authenticationManager)
    {
        _logger = logger;
        _authenticationManager = authenticationManager;
    }
    [AllowAnonymous]
    [HttpGet("/login")]
    public string Login()
    {
        return _authenticationManager.CreateToken(new SecurityPrincipal
        {
            Id = 1,
            UserType = UserType.Admin
        });
    }
    
    
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast

        {
            Date = DateTime.Now.AddDays(index), TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }
        );
    }
}

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}