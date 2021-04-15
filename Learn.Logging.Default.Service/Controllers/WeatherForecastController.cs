using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Learn.Logging.Default.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = {
              "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogTrace("log trace");
            _logger.LogDebug("log debug");
            _logger.LogInformation("log information");
            _logger.LogWarning("log warning");

            try
            {
                throw new InvalidOperationException("Some error");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "log error");
                _logger.LogCritical(e, "log critical error");
            }

            //_logger.LogInformation("log some data: {@a} and {@b}", new User { Id = 123, Name = "John" }, Summaries);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
              .ToArray();
        }
    }

    class User
    {
        public int Id { get; set; }
        public string Name;
    }
}