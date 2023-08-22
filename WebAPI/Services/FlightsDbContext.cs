// <copyright file="FlightsDbContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI
{
	using Microsoft.EntityFrameworkCore;

	public class FlightsDbContext : DbContext
	{
		public FlightsDbContext(DbContextOptions<FlightsDbContext> options)
			: base(options)
		{
		}

		public DbSet<Flight> Flights { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Flight>().Property(e => e.DepartureCity).HasConversion<string>();
			modelBuilder.Entity<Flight>().Property(e => e.ArrivalCity).HasConversion<string>();
			modelBuilder.Entity<Flight>().Property(e => e.Airline).HasConversion<string>();

			base.OnModelCreating(modelBuilder);
		}
	}
}
