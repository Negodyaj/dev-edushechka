using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("weather-forecast")]
    public class WeatherForecastController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //  /weather-forecast
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //  /weather-forecast/universal-answer
        [HttpGet("universal-answer/{number}/{number2}")]
        public int Get42(int number, int number2)
        {
            return number * number2;
        }

        //  /student/5/task/2
        [HttpGet("student/{studentId}/task/{taskId}")]
        public string GetStudentTask(int studentId, int taskId)
        {
            return $"bla-bla-bla {studentId} {taskId}";
        }

        /*
         * 
         * GET - получить что-то
         * POST - добавить новую запись (в БД)
         * PUT - обновить имеющуюся запись (дёрнуть какой-то update)
         * DELETE - удалить запись
         * PATCH - частичное обновление имеющейся записи
         * 
         */
    }
}
