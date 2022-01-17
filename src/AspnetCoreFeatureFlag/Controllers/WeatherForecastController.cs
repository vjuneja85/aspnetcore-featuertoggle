using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace AspnetCoreFeatureFlag.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Swelter", "Scorch"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    public  IFeatureManager _featuremanager { get; }

    public WeatherForecastController(IFeatureManager featureManager,
        ILogger<WeatherForecastController> logger)
    {
        _featuremanager = featureManager;
        _logger = logger;
    }

    [HttpGet("GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var rng = new Random();
        
        var _isRainEnabled = await _featuremanager.IsEnabledAsync("RainEnabled");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            RainExpected = _isRainEnabled ? $"{rng.Next(0,100)}%" : null,
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }


    [HttpGet("GetWeatherForecastExtended")]
    [FeatureGate("ExtendedEnabled")]
    public async Task<IEnumerable<WeatherForecast>> GetExtended()
    {
        var useNewAlgorithm = await _featuremanager.IsEnabledAsync("NewAlgorithmEnabled");
        return useNewAlgorithm 
        ? await NewAlgorithm()
        : await OldAlgorithm();
    }


    private async Task<IEnumerable<WeatherForecast>> OldAlgorithm()
    {
        var rng = new Random();
        
        var _isRainEnabled = await _featuremanager.IsEnabledAsync("RainEnabled");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            RainExpected = _isRainEnabled ? $"{rng.Next(0,100)}% OLD" : null,
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }


    private async Task<IEnumerable<WeatherForecast>> NewAlgorithm()
    {
        var rng = new Random();
        
        var _isRainEnabled = await _featuremanager.IsEnabledAsync("RainEnabled");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            RainExpected = _isRainEnabled ? $"{rng.Next(0,100)}% NEW" : null,
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }


}
