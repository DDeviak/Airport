// <copyright file="HttpContentNewtonsoftJsonExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WebAPI.Tests
{
	using Newtonsoft.Json;
	public static class HttpContentNewtonsoftJsonExtensions
	{
		public static async Task<T?> ReadFromJsonAsync<T>(this HttpContent content, JsonSerializer serializer)
		{
			string json = await content.ReadAsStringAsync();
			return (T?)await Task.Run(() => serializer.Deserialize(new StringReader(json), typeof(T)));
		}
	}
}
