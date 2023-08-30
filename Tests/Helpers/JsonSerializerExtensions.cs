// <copyright file="JsonSerializerExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI.Tests
{
    using Newtonsoft.Json;

	public static class JsonSerializerExtensions
	{
		public static string Serialize(this JsonSerializer serializer, object? obj)
		{
			StringWriter sw = new StringWriter();
			serializer.Serialize(sw, obj);
			return sw.ToString();
		}

	}
}
