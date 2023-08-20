
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
            return Ok(await Task.Run(() => Program.Db.Flights.AsEnumerable()));
        }

        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Flight))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            Flight? item = await Program.Db.FindAsync<Flight>(id);
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
            if ((await Program.Db.FindAsync<Flight>(value.ID)) != null) return BadRequest();

            await Program.Db.AddAsync(value);
            await Program.Db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = value.ID }, value);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] Flight value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (value == null) return BadRequest(value);

            Flight? item = await Program.Db.FindAsync<Flight>(value.ID);
            if (item != null) await Task.Run(() => Program.Db.Remove(item));

            await Program.Db.AddAsync(value);
            await Program.Db.SaveChangesAsync();

            if (item == null)
            {
                return CreatedAtAction(nameof(Get), new { id = value.ID }, value);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Flight> patch)
        {
            Flight? item = await Program.Db.FindAsync<Flight>(id);

            if (item == null) return NotFound();

            patch.ApplyTo(item, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Program.Db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            Flight? item = await Program.Db.FindAsync<Flight>(id);
            if (item != null)
            {
                await Task.Run(() => Program.Db.Remove(item));
                await Program.Db.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
