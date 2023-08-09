using Airport;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        // GET: api/<CountryController>
        [HttpGet]
        public IEnumerable<Country> Get()
        {
            return Country.Countries.Values.ToList();
        }

        // GET api/<CountryController>/5
        [HttpGet("{name}")]
        public Country Get(string name)
        {
            return Country.Countries[name];
        }
    }
}
