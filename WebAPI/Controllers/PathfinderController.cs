// <copyright file="PathfinderController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI.Controllers
{
	using System.Net.Mime;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/[controller]")]
	[ApiController]
	public class PathfinderController : ControllerBase
	{
		[HttpGet]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Flight>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromQuery] City departureCity, [FromQuery] City arrivalCity, [FromQuery] DateOnly departureDate)
		{
			try
			{
				IEnumerable<Flight>? path = await Task.Run(() => Program.Pathfinder.GetPath(departureCity, arrivalCity, departureDate.ToDateTime(default(TimeOnly))));
				if (path != null)
				{
					return Ok(path);
				}
				else
				{
					return NotFound();
				}
			}
			catch (ArgumentException)
			{
				return BadRequest();
			}
		}

		[HttpGet("byCountry")]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDictionary<City, IEnumerable<Flight>?>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromQuery] City departureCity, [FromQuery] string arrivalCountry, [FromQuery] DateOnly departureDate)
		{
			try
			{
				IDictionary<City, IEnumerable<Flight>?> pathes = await Task.Run(() => Program.Pathfinder.GetFlightsToCountry(departureCity, arrivalCountry, departureDate.ToDateTime(default(TimeOnly))));
				if (pathes.Count != 0)
				{
					return Ok(pathes);
				}
				else
				{
					return NotFound();
				}
			}
			catch (ArgumentException)
			{
				return BadRequest();
			}
		}

		[HttpGet("byMonth")]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDictionary<DateTime, IEnumerable<Flight>?>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromQuery] City departureCity, [FromQuery] City arrivalCity, [FromQuery] int year, [FromQuery] int month)
		{
			try
			{
				IDictionary<DateTime, IEnumerable<Flight>?> pathes = await Task.Run(() => Program.Pathfinder.GetFlightsByMonth(departureCity, arrivalCity, year, month));
				if (pathes.Any(t => t.Value != null))
				{
					return Ok(pathes);
				}
				else
				{
					return NotFound();
				}
			}
			catch (ArgumentException)
			{
				return BadRequest();
			}
		}
	}
}
