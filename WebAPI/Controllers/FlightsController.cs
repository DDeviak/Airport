using Airport;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        // GET: api/<FlightsController>
        [HttpGet]
        public GraphCollection Get()
        {
            return Program.Pathfinder.Graph;
        }
        // GET: api/<FlightsController>/5
        [HttpGet("{id}")]
        public Flight Get(int id)
        {
            return Program.Pathfinder.Graph.Get(id);
        }
        // POST api/<FlightsController>
        [HttpPost]
        public void Post([FromBody] Flight value)
        {
            Program.Pathfinder.Graph.Add(value);
        }
        // PATCH api/<FlightsController>/5
        [HttpPatch("{id}")]
        public void Patch(int id, [FromBody] Changes value)
        {
            Program.Pathfinder.Graph.Modify(id, value.FieldName, value.Value);
        }
        public record Changes(string FieldName, object Value);
        // DELETE api/<FlightsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Program.Pathfinder.Graph.Remove(id);
        }
    }
}
