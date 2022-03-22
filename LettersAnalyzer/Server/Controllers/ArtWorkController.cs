using LettersAnalyzer.Shared;
using Microsoft.AspNetCore.Mvc;

namespace LettersAnalyzer.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtWorkController : ControllerBase
    {
        private readonly ILogger<ArtWorkController> _logger;

        public ArtWorkController(ILogger<ArtWorkController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 6).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
            })
            .ToArray();
        }
    }
}