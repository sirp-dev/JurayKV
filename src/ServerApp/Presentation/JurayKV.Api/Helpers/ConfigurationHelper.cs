﻿using System;

namespace JurayKV.Api;

public static class ConfigurationHelper
{
	public static string GetDbConnectionString(this WebApplicationBuilder builder)
	{
		ArgumentNullException.ThrowIfNull(builder);

		bool isUnixLikeSystem = Environment.OSVersion.Platform == PlatformID.Unix
								|| Environment.OSVersion.Platform == PlatformID.MacOSX;

		string connectionString;

		if (isUnixLikeSystem)
		{
			connectionString = builder.Configuration.GetConnectionString("DockerDbConnection");
		}
		else
		{
			string connectionName = builder.Environment.IsDevelopment() ? "DefaultConnection" : "DefaultConnection";
			connectionString = builder.Configuration.GetConnectionString(connectionName);
		}

		return connectionString;
	}
}
