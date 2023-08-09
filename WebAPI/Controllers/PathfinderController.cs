using Airport;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PathfinderController : ControllerBase
    {
        // GET: api/<PathfinderController>
        [HttpGet]
        public IEnumerable<Flight> Get([FromQuery] City departureCity, [FromQuery] City arrivalCity, [FromQuery] DateOnly departureDate)
        {
            return Program.Pathfinder.GetPath(departureCity, arrivalCity, departureDate.ToDateTime(new TimeOnly()));
        }
        // GET: api/<PathfinderController>/byCountry
        [HttpGet("byCountry")]
        public IEnumerable<KeyValuePair<City,IEnumerable<Flight>?>> Get([FromQuery] City departureCity, [FromQuery] string arrivalCountry, [FromQuery] DateOnly departureDate)
        {
            return Program.Pathfinder.GetFlightsToCountry(departureCity, arrivalCountry, departureDate.ToDateTime(new TimeOnly()));
        }
        // GET: api/<PathfinderController>/byMonth
        [HttpGet("byMonth")]
        public IEnumerable<KeyValuePair<DateTime, IEnumerable<Flight>?>> Get([FromQuery] City departureCity, [FromQuery] City arrivalCity, [FromQuery] int year, [FromQuery]int month)
        {
            return Program.Pathfinder.GetFlightsByMonth(departureCity, arrivalCity, year, month);
        }
    }
}
