using System.Collections.Generic;

namespace heng
{
	public static class Log
	{
		static readonly Dictionary<ILogger, LogLevel> loggers;

		static Log() => loggers = new Dictionary<ILogger, LogLevel>();

		public static void AddLogger(ILogger logger, LogLevel minLevel) => loggers.Add(logger, minLevel);
		public static void RemoveLogger(ILogger logger) => loggers.Remove(logger);
		public static void ClearLoggers() => loggers.Clear();

		public static void Debug(string msg) => Print(LogLevel.Debug, msg);
		public static void Note(string msg) => Print(LogLevel.Note, msg);
		public static void Warning(string msg) => Print(LogLevel.Warning, msg);
		public static void Error(string msg) => Print(LogLevel.Error, msg);
		public static void Failure(string msg) => Print(LogLevel.Failure, msg);

		public static void Print(LogLevel level, string msg)
		{
			foreach(var pair in loggers)
			{
				if(level >= pair.Value)
				{ pair.Key.Print(level, msg); }
			}
		}
	};
}