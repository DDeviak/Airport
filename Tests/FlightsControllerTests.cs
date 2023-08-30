// <copyright file="FlightsControllerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI.Tests
{
	using System.Net.Mime;
	using Microsoft.AspNetCore.JsonPatch;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	public class FlightsControllerTests : TestBase
	{
		private static TheoryData<Flight> testData = null!;

		private readonly Flight invalidFlight = new Flight
		{
			ID = 0,
			DepartureCity = City.Chicago,
			ArrivalCity = City.Chicago,
			DepartureDatetime = new DateTime(2023, 07, 15, 00, 00, 00, DateTimeKind.Utc),
			ArrivalDatetime = new DateTime(2023, 07, 10, 00, 00, 00, DateTimeKind.Utc),
			Airline = Airline.Delta,
			Price = -1,
		};

		public static TheoryData<Flight> TestData
		{
			get
			{
				if (testData is null)
				{
					using (Stream stream = File.OpenRead(Directory.GetCurrentDirectory() + "\\testData.json") ?? Stream.Null)
					using (StreamReader sr = new StreamReader(stream))
					{
						JsonSerializer jsonSerializer = new JsonSerializer();

						jsonSerializer.Converters.Add(new StringEnumConverter());
						jsonSerializer.DateFormatString = "dd/MM/yyyy HH:mm:ss";
						jsonSerializer.Formatting = Newtonsoft.Json.Formatting.Indented;

						testData = new TheoryData<Flight>();
						((List<Flight>?)jsonSerializer.Deserialize(sr, typeof(List<Flight>)) ?? new List<Flight>()).ForEach(t => testData.Add(t));
					}
				}

				return testData;
			}
		}

		[Fact]
		public async Task GetAll_OnEmpty_Test()
		{
			// Arrange
			foreach (object[] t in TestData)
			{
				Flight? entity = await testDb.Flights.FindAsync(((Flight)t[0]).ID);
				if (entity is not null)
				{
					testDb.Flights.Remove(entity);
				}
			}

			await testDb.SaveChangesAsync();

			// Act
			var response = await testClient.GetAsync("api/Flights");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			(await response.Content.ReadFromJsonAsync<List<Flight>>(serializer)).Should().BeEmpty();
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Get_OnEmpty_Test(Flight item)
		{
			// Arrange
			Flight? entity = await testDb.Flights.FindAsync(item.ID);
			if (entity is not null)
			{
				testDb.Flights.Remove(entity);
			}

			await testDb.SaveChangesAsync();

			// Act
			var response = await testClient.GetAsync($"api/Flights/{item.ID}");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task GetAll_OnNotEmpty_Test()
		{
			// Arrange
			foreach (object[] t in TestData)
			{
				testDb.Flights.Add((Flight)t[0]);
			}

			await testDb.SaveChangesAsync();

			// Act
			var response = await testClient.GetAsync("api/Flights");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			(await response.Content.ReadFromJsonAsync<List<Flight>>(serializer)).Should().NotBeNullOrEmpty();
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Get_OnNotEmpty_Test(Flight item)
		{
			// Arrange
			testDb.Flights.Add(item);
			await testDb.SaveChangesAsync();

			// Act
			var response = await testClient.GetAsync($"api/Flights/{item.ID}");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			(await response.Content.ReadFromJsonAsync<Flight>(serializer)).Should().NotBeNull();
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Post_Test(Flight item)
		{
			// Arrange
			Flight? entity = await testDb.Flights.FindAsync(item.ID);
			if (entity is not null)
			{
				testDb.Flights.Remove(entity);
			}

			await testDb.SaveChangesAsync();

			HttpContent content = new StringContent(serializer.Serialize(item), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var request = await testClient.PostAsync("api/Flights", content);

			// Assert
			request.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
		}

		[Fact]
		public async Task Post_Invalid_Test()
		{
			// Arrange
			HttpContent content = new StringContent(serializer.Serialize(invalidFlight), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var request = await testClient.PostAsync("api/Flights", content);

			// Assert
			request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Post_Null_Test()
		{
			// Arrange
			HttpContent content = new StringContent(serializer.Serialize(null), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var request = await testClient.PostAsync("api/Flights", content);

			// Assert
			request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Put_Invalid_Test()
		{
			// Arrange
			HttpContent content = new StringContent(serializer.Serialize(invalidFlight), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var request = await testClient.PutAsync("api/Flights", content);

			// Assert
			request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Put_Null_Test()
		{
			// Arrange
			HttpContent content = new StringContent(serializer.Serialize(null), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var request = await testClient.PutAsync("api/Flights", content);

			// Assert
			request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Put_OnEmpty_Test(Flight item)
		{
			// Arrange
			Flight? entity = await testDb.Flights.FindAsync(item.ID);
			if (entity is not null)
			{
				testDb.Flights.Remove(entity);
			}

			await testDb.SaveChangesAsync();

			HttpContent content = new StringContent(serializer.Serialize(item), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var request = await testClient.PutAsync("api/Flights", content);

			// Assert
			request.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Put_OnNotEmpty_Test(Flight item)
		{
			// Arrange
			testDb.Flights.Add(item);
			await testDb.SaveChangesAsync();

			HttpContent content = new StringContent(serializer.Serialize(item), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var response = await testClient.PutAsync("api/Flights", content);

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Patch_Invalid_Test(Flight item)
		{
			// Arrange
			testDb.Flights.Add(item);
			await testDb.SaveChangesAsync();

			JsonPatchDocument<Flight> jsonPatch = new JsonPatchDocument<Flight>();
			jsonPatch.Replace(t => t.ArrivalDatetime, item.DepartureDatetime);

			HttpContent content = new StringContent(serializer.Serialize(jsonPatch), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var request = await testClient.PatchAsync($"api/Flights/{item.ID}", content);

			// Assert
			request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Patch_Null_Test(Flight item)
		{
			// Arrange
			HttpContent content = new StringContent(serializer.Serialize(null), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var request = await testClient.PatchAsync($"api/Flights/{item.ID}", content);

			// Assert
			request.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Patch_OnEmpty_Test(Flight item)
		{
			// Arrange
			Flight? entity = await testDb.Flights.FindAsync(item.ID);
			if (entity is not null)
			{
				testDb.Flights.Remove(entity);
			}

			await testDb.SaveChangesAsync();

			JsonPatchDocument<Flight> jsonPatch = new JsonPatchDocument<Flight>();
			jsonPatch.Replace(t => t.Price, 256);

			HttpContent content = new StringContent(serializer.Serialize(jsonPatch), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var request = await testClient.PatchAsync($"api/Flights/{item.ID}", content);

			// Assert
			request.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Patch_OnNotEmpty_Test(Flight item)
		{
			// Arrange
			testDb.Flights.Add(item);
			await testDb.SaveChangesAsync();

			JsonPatchDocument<Flight> jsonPatch = new JsonPatchDocument<Flight>();
			jsonPatch.Replace(t => t.Price, 256);

			HttpContent content = new StringContent(serializer.Serialize(jsonPatch), new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json));

			// Act
			var response = await testClient.PatchAsync($"api/Flights/{item.ID}", content);

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Delete_Test(Flight item)
		{
			// Arrange

			// Act
			var response = await testClient.DeleteAsync($"api/Flights/{item.ID}");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public async Task Delete_OnNotEmpty_Test(Flight item)
		{
			// Arrange
			testDb.Flights.Add(item);
			await testDb.SaveChangesAsync();

			// Act
			var response = await testClient.DeleteAsync($"api/Flights/{item.ID}");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
		}
	}
}
