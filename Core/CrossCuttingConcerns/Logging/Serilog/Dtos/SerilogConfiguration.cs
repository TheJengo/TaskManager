using System.Collections.Generic;

namespace Core.CrossCuttingConcern.Logging.Serilog.Dtos
{
	public class SerilogConfiguration
	{
		public MinimumLevel MinimumLevel { get; set; }
		public List<string> Enrich { get; set; }
		public Properties Properties { get; set; }
		public List<WriteTo> WriteTo { get; set; }
	}

	public class Override
	{
		public string Microsoft { get; set; }
		public string System { get; set; }
	}

	public class MinimumLevel
	{
		public string Default { get; set; }
		public Override Override { get; set; }
	}

	public class Properties
	{
		public string Application { get; set; }
		public string Version { get; set; }
		public string MachineName { get; set; }
	}

	public class Args
	{
		public string Path { get; set; }
		public string OutputTemplate { get; set; }
		public string RollingInterval { get; set; }
		public string ConnectionString { get; set; }
		public string TableName { get; set; }
		public string RestrictedToMinimumLevel { get; set; }
		public System.TimeSpan Period { get; set; }
	}

	public class WriteTo
	{
		public string Name { get; set; }
		public Args Args { get; set; }
	}
}
