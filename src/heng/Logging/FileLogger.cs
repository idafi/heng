namespace heng.Logging
{
	/// <summary>
	/// Logs messages to the engine core's output file.
	/// </summary>
	public class FileLogger : ILogger
	{
		/// <inheritdoc />
		public void Print(LogLevel level, string msg) => Core.Log.File.Print(level, msg);
	};
}