using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoPractice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
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

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://<collection>:<pass>@mongomicroservices.pvfbh.mongodb.net/<dbname>?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("MongoPracticeDb");

            var collection = database.GetCollection<Test>("Test");

            var test = new Test()
            {
                _Id = ObjectId.GenerateNewId(),
                Name = "Test Verisi",
                Age = 30,
                AddressList = new List<Address>()
                {
                    new Address
                    {
                        Title = "Ev Adresi",
                        Description = "Ev adresi bilgileri"
                    },
                    new Address
                    {
                        Title = "İş Adresi",
                        Description = "İş adresi bilgileri"
                    }
                }
            };
            collection.InsertOne(test);


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
    public class Test
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ICollection<Address> AddressList { get; set; }
    }

    public class Address
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}