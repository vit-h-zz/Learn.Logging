using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Learn.Logging.Service2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("fetch")]
        public async Task<IEnumerable<WeatherForecast>> FetchFromRemote()
        {
            // 1. Structured logging
            var user = new User {Id = 123, Name = "James Bond", Prefix = "Bond"};
            _logger.LogInformation("User {@User} requested weather forecast", user);
            _logger.LogInformation("User {User} without @", user);

            // 2. Enrich nested logs with scope data
            var dictionary = new Dictionary<string, dynamic>
            {
                {"City", "London"},
                {"IsVipUser", true},
            };

            using (_logger.BeginScope(dictionary))
            {
                _logger.LogInformation("User {@User} requested weather forecast", user);

                var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5003") };
                var forecast = await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("/weatherforecast");

                //call more to see the APM change
                //forecast = await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("/weatherforecast");

                return forecast;
            }
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var dictionary = new Dictionary<string, dynamic>
            {
                {"City", "Los Angeles"},
                {"IsVipUser", true},
            };

            using (_logger.BeginScope(dictionary))
            {
                var user = new User { Id = 124, Name = "Jackie Chan", Prefix = "Mr" };
                _logger.LogInformation("User {@User} requested weather forecast", user);

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
    }

    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prefix;
    }
}