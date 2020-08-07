using System;

namespace Core.CrossCuttingConcern.Logging.Serilog.Dtos
{
	public class LogChange
	{
		public string Id { get; set; }
		public string LogLevel { get; set; }
		public string Entity { get; set; }
		public int EntityId { get; set; }
		public System.Collections.Generic.List<Logs> Logs { get; set; }
	}

	public class Logs
	{
		public User User { get; set; }
		public ChangeLog ChangeLog { get; set; }
		public Information Informations { get; set; }
		public DateTime TimeStamp { get; set; } = DateTime.Now;
		public DateTime UtcTimeStamp { get; set; } = DateTime.UtcNow;
		public string State { get; set; }
	}

	public class User
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Token { get; set; }
	}

	public class ChangeLog
	{
		public string State { get; set; }
		public dynamic OriginalValues { get; set; }
		public dynamic ModifiedValues { get; set; }
	}

	public class Information
	{
		public string Action { get; set; }
		public string ActionId { get; set; }
		public string Application { get; set; }
		public string Controller { get; set; }
		public string Culture { get; set; }
		public string CultureUI { get; set; }
		public string DomainName { get; set; }
		public string MachineName { get; set; }
		public string OSVersion { get; set; }
		public string Page { get; set; }
		public string RefererUrl { get; set; }
		public string RequestIp { get; set; }
		public string RequestUrl { get; set; }
		public string StackTrace { get; set; }
		public string UserAgent { get; set; }
		public string Version { get; set; }
		public System.Collections.Generic.IDictionary<string, string> RouteValues { get; set; }
	}
}
