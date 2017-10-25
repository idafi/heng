namespace heng
{
	public class FileLogger : ILogger
	{
		public void Print(LogLevel level, string msg) => Core.Log.File.Print(level, msg);
	};
}