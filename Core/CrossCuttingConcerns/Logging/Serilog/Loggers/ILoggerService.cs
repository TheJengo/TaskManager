using Core.CrossCuttingConcern.Logging.Serilog.Dtos;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcern.Logging.Serilog.Loggers
{
	public interface ILoggerService : IDisposable
	{

		void LogInformation(string message, params object[] parameters);
		void LogInformation(LogChange log);

		void LogWarning(string message);
		void LogWarning(LogChange log);

		void LogDebug(string message);
		void LogDebug(LogChange log);

		void LogError(string message);
		void LogError(LogChange log);

		void LogFatal(string message);
		void LogFatal(LogChange log);

		void LogVerbose(string message);
		void LogVerbose(LogChange log);

		void Write(LogEventLevel level, Exception ex, string template);
	}
}
