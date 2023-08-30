// <copyright file="FlightsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI.Controllers
{
	using System.Net.Mime;
	using Microsoft.AspNetCore.JsonPatch;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/[controller]")]
	[ApiController]
	public class FlightsController : ControllerBase
	{
		private readonly FlightsDbContext context;

		public FlightsController(FlightsDbContext context)
		{
			this.context = context;
		}

		[HttpGet]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Flight>))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await Task.Run(() => context.Flights.AsEnumerable()));
		}

		[HttpGet("{id}")]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Flight))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(int id)
		{
			Flight? item = await context.FindAsync<Flight>(id);
			return item != null ? Ok(item) : NotFound();
		}

		[HttpPost]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Post([FromBody] Flight value)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (value == null)
			{
				return BadRequest(value);
			}

			if ((await context.FindAsync<Flight>(value.ID)) != null)
			{
				return BadRequest();
			}

			await context.AddAsync(value);
			await context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { id = value.ID }, value);
		}

		[HttpPut]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Put([FromBody] Flight value)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (value == null)
			{
				return BadRequest(value);
			}

			Flight? item = await context.FindAsync<Flight>(value.ID);
			if (item != null)
			{
				await Task.Run(() => context.Remove(item));
			}

			await context.AddAsync(value);
			await context.SaveChangesAsync();

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
			Flight? item = await context.FindAsync<Flight>(id);

			if (item == null)
			{
				return NotFound();
			}

			patch.ApplyTo(item, ModelState);
			TryValidateModel(item);
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await context.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> Delete(int id)
		{
			Flight? item = await context.Flights.FindAsync(id);
			if (item is not null)
			{
				await Task.Run(() => context.Flights.Remove(item));
				await context.SaveChangesAsync();
			}

			return NoContent();
		}
	}
}
