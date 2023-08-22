// <copyright file="Airline.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI
{
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	[JsonConverter(typeof(StringEnumConverter))]
	public enum Airline
	{
		Undefined = 0,
		Delta,
		AmericanAirlines,
		United,
		Southwest,
		AirFrance,
		Lufthansa,
		BritishAirways,
		Emirates,
		CathayPacific,
		Qantas,
		AirCanada,
		SingaporeAirlines,
		TurkishAirlines,
		ANA,
		JapanAirlines,
		KoreanAir,
		EtihadAirways,
		VirginAtlantic,
		QatarAirways,
		JetBlue,
	}
}
