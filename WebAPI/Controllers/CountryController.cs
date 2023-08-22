// <copyright file="CountryController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI.Controllers
{
	using System.Net.Mime;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/[controller]")]
	[ApiController]
	public class CountryController : ControllerBase
	{
		[HttpGet]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Country>))]
		public async Task<IActionResult> Get()
		{
			return Ok(await Task.Run(() => Country.GetAll()));
		}

		[HttpGet("{name}")]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Country))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(string name)
		{
			try
			{
				return Ok(await Task.Run(() => Country.Get(name)));
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
		}
	}
}
