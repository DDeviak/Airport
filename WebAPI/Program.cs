// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI
{
	using Microsoft.EntityFrameworkCore;
	using Newtonsoft.Json.Converters;

	public static class Program
	{
		public static FlightsDbContext Db { get; private set; } = null!;

		public static FlightsPathfinder Pathfinder { get; private set; } = null!;

		public static void Main(string[] args)
		{
			var configBuilder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");
			var config = configBuilder.Build();
			string connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;

			var optionsBuilder = new DbContextOptionsBuilder<FlightsDbContext>();
			var options = optionsBuilder.UseNpgsql(connectionString).Options;

			var builder = WebApplication.CreateBuilder(args);

			Db = new FlightsDbContext(options);
			Pathfinder = new FlightsPathfinder(new DbGraphProvider(Db));

			builder.Services.AddControllers().AddNewtonsoftJson(opts =>
			{
				opts.SerializerSettings.Converters.Add(new StringEnumConverter());
				opts.SerializerSettings.DateFormatString = "dd/MM/yyyy HH:mm:ss";
				opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
			});
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen().AddSwaggerGenNewtonsoftSupport();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.MapControllers();

			app.Run();
		}
	}
}