namespace heng
{
	public class ConsoleLogger : ILogger
	{
		public void Print(LogLevel level, string msg) => Core.Log.Console.Print(level, msg);
	};
}