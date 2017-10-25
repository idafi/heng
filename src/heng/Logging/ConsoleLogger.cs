namespace heng.Logging
{
	/// <summary>
	/// Logs messages to the engine core's console.
	/// </summary>
	public class ConsoleLogger : ILogger
	{
		/// <inheritdoc />
		public void Print(LogLevel level, string msg) => Core.Log.Console.Print(level, msg);
	};
}