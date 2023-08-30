// <copyright file="PathfinderControllerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI.Tests
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

	public class PathfinderControllerTests : TestBase
	{
		public PathfinderControllerTests()
            : base()
		{
			using (Stream stream = File.OpenRead(Directory.GetCurrentDirectory() + "\\testData.json") ?? Stream.Null)
			using (StreamReader sr = new StreamReader(stream))
			{
				JsonSerializer jsonSerializer = new JsonSerializer();

				jsonSerializer.Converters.Add(new StringEnumConverter());
				jsonSerializer.DateFormatString = "dd/MM/yyyy HH:mm:ss";
				jsonSerializer.Formatting = Newtonsoft.Json.Formatting.Indented;

				((List<Flight>?)jsonSerializer.Deserialize(sr, typeof(List<Flight>)) ?? new List<Flight>()).ForEach(t =>
				{
					testDb.Add(t);
				});
				testDb.SaveChanges();
			}
		}

		[Fact]
		public async Task GetPath_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Pathfinder?departureCity=Chicago&arrivalCity=Philadelphia&departureDate=2023-07-14");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			(await response.Content.ReadFromJsonAsync<List<Flight>>(serializer)).Should().NotBeNullOrEmpty();
		}

		[Fact]
		public async Task GetPath_Invalid_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Pathfinder?departureCity=Philadelphia&arrivalCity=Philadelphia&departureDate=2023-07-14");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task GetPath_NotFound_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Pathfinder?departureCity=LosAngeles&arrivalCity=Austin&departureDate=2023-07-14");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task GetPathByCountry_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Pathfinder/byCountry?departureCity=Chicago&arrivalCountry=UnitedStates&departureDate=2023-07-14");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			Dictionary<City, IEnumerable<Flight>>? pathesByCity = await response.Content.ReadFromJsonAsync<Dictionary<City, IEnumerable<Flight>>>(serializer);
			pathesByCity.Should().NotBeNullOrEmpty();
			foreach (IEnumerable<Flight> path in pathesByCity.Values)
			{
				path.Should().NotBeNullOrEmpty();
			}
		}

		[Fact]
		public async Task GetPathByCountry_Invalid_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Pathfinder/byCountry?departureCity=Chicago&arrivalCountry=InvalidCountry&departureDate=2023-07-14");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task GetPathByCountry_NotFound_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Pathfinder/byCountry?departureCity=Chicago&arrivalCountry=UnitedStates&departureDate=1000-07-14");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task GetPathByMonth_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Pathfinder/byMonth?departureCity=Chicago&arrivalCity=Philadelphia&year=2023&month=07");
			Dictionary<DateTime, IEnumerable<Flight>?>? pathesByDate = await response.Content.ReadFromJsonAsync<Dictionary<DateTime, IEnumerable<Flight>?>>(serializer);

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			pathesByDate.Should().NotBeNullOrEmpty();
			pathesByDate?.Any(t => t.Value is not null).Should().BeTrue();
		}

		[Fact]
		public async Task GetPathByMonth_Invalid_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Pathfinder/byMonth?departureCity=Philadelphia&arrivalCity=Philadelphia&year=2023&month=07");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task GetPathByMonth_NotFound_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Pathfinder/byMonth?departureCity=LosAngeles&arrivalCity=Austin&year=2023&month=07");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		}
	}
}
