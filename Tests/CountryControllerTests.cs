// <copyright file="CountryControllerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI.Tests
{
	public class CountryControllerTests : TestBase
	{
		[Fact]
		public async Task GetAll_Test()
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync("api/Country");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			(await response.Content.ReadFromJsonAsync<List<Flight>>()).Should().NotBeNullOrEmpty();
		}

		[Theory]
		[InlineData("Canada")]
		[InlineData("UnitedStates")]
		public async Task Get_Success_Test(string name)
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync($"api/Country/{name}");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			(await response.Content.ReadFromJsonAsync<object>()).Should().NotBeNull();
		}

		[Theory]
		[InlineData("InvalidCountry")]
		public async Task Get_Failure_Test(string name)
		{
			// Arrange

			// Act
			var response = await testClient.GetAsync($"api/Country/{name}");

			// Assert
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
		}
	}
}
