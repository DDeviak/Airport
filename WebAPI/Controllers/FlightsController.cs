
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Flight>))]
        public async Task<IActionResult> Get()
        {
            return Ok(await Task.Run(() => Program.db.Flights.AsEnumerable()));
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Flight))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            Flight? item = await Program.db.FindAsync<Flight>(id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Flight value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (value == null) return BadRequest(value);
            if ((await Program.db.FindAsync<Flight>(value.ID)) != null) return BadRequest();

            await Program.db.AddAsync(value);
            await Program.db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = value.ID }, value);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] Flight value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (value == null) return BadRequest(value);

            Flight? item = await Program.db.FindAsync<Flight>(id);
            if (item != null) await Task.Run(() => Program.db.Remove(item));

            await Program.db.AddAsync(value);
            await Program.db.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Flight> patch)
        {
            Flight? item = await Program.db.FindAsync<Flight>(id);

            if (item == null) return NotFound();

            patch.ApplyTo(item, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Program.db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await Task.Run(() => Program.db.Remove(id));
            await Program.db.SaveChangesAsync();
            return NoContent();
        }
    }
}
