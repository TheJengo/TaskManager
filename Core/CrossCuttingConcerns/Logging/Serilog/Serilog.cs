using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.CrossCuttingConcerns.Logging.Serilog
{
    public class Serilog
    {
		public static void LogInformation(string message)
		{
			//var logg = new LoggerConfiguration();
			//logg.WriteTo.Console();
			//logg.MinimumLevel.Debug();


			using (var log = new LoggerConfiguration()
				.ReadFrom.Configuration(new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
					.Build(), "Serilog", null)
				.CreateLogger())
			{
				log.Information("information");
				log.Warning("warning");
				log.Debug("debug");
			}
		}
	}
}
