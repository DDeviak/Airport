// <copyright file="TestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI.Tests
{
	using Microsoft.AspNetCore.Mvc.Testing;
	using Microsoft.Data.Sqlite;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	public class TestBase
	{
		protected readonly HttpClient testClient;

		protected readonly JsonSerializer serializer;

		protected readonly SqliteConnection testConnection;

		protected readonly FlightsDbContext testDb;

		protected TestBase()
		{
			testConnection = new SqliteConnection("Filename=:memory:");
			testConnection.Open();

			var appFactory = new WebApplicationFactory<Program>()
				.WithWebHostBuilder(config =>
			{
				config.ConfigureServices(services =>
				{
					services.RemoveAll(typeof(DbContextOptions<FlightsDbContext>));

					services.AddDbContext<FlightsDbContext>(options =>
					{
						options.UseSqlite(testConnection);
					});
				});
			});

			testDb = new FlightsDbContext(new DbContextOptionsBuilder<FlightsDbContext>().UseSqlite(testConnection).Options);
			testDb.Database.Migrate();

			testClient = appFactory.CreateClient();

			serializer = new JsonSerializer();

			serializer.Converters.Add(new StringEnumConverter());
			serializer.DateFormatString = "dd/MM/yyyy HH:mm:ss";
			serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
		}

		public void Dispose()
		{
			testDb.Database.EnsureDeleted();
			testDb.Dispose();
			testConnection.Dispose();
		}
	}
}
