using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Infrastructure.Logging
{
    public class LogConfigs
    {
		/// <summary>
		/// Common logging configuration for microservices
		/// </summary>
		public static void ConfigureLogging(string assemblyName, IConfigurationRoot configuration, string environment)
		{
			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
				.WriteTo.Elasticsearch(ConfigureElasticSink(assemblyName, configuration, environment))
				.Enrich.WithProperty("Environment", environment)
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			Serilog.Debugging.SelfLog.Enable(Console.Error);
		}

		private static ElasticsearchSinkOptions ConfigureElasticSink(string assemblyName, IConfigurationRoot configuration, string environment)
		{
			return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
			{
				AutoRegisterTemplate = true,
				IndexFormat = $"{assemblyName.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
			};
		}
	}
}
