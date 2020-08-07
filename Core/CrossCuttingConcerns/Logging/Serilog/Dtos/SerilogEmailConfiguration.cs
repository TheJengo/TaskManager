namespace Core.CrossCuttingConcern.Logging.Serilog.Dtos.Email
{
	public class SerilogEmailConfiguration
	{
		public string Name { get; set; }
		public Args Args { get; set; }
	}

	public class NetworkCredentials
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class Args
	{
		public string FromEmail { get; set; }
		public string ToEmail { get; set; }
		public string MailServer { get; set; }
		public NetworkCredentials NetworkCredentials { get; set; }
		public int Port { get; set; }
		public bool EnableSsl { get; set; }
		public bool IsBodyHtml { get; set; }
		public int BatchPostingLimit { get; set; }
		public string MailSubject { get; set; }
		public string OutputTemplate { get; set; }
		public string RestrictedToMinimumLevel { get; set; }
	}
}
