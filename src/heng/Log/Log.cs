using System.Collections.Generic;

namespace heng
{
	public static class Log
	{
		static readonly Dictionary<ILogger, LogLevel> loggers;

		static Log() => loggers = new Dictionary<ILogger, LogLevel>();

		public static void AddLogger(ILogger logger, LogLevel minLevel)
		{
			if(logger != null)
			{
				if(!loggers.ContainsKey(logger))
				{ loggers.Add(logger, minLevel); }
				else
				{ Log.Error("couldn't add logger: logger is already added"); }
			}
			else
			{ Log.Error("couldn't add logger: logger is null"); }
		}
		public static void RemoveLogger(ILogger logger)
		{
			if(logger != null)
			{
				if(!loggers.Remove(logger))
				{ Log.Warning("couldn't add logger: logger isn't yet added, or was already removed"); }
			}
			else
			{ Log.Warning("couldn't remove logger: logger was null"); }
		}

		public static void ClearLoggers()
		{
			loggers.Clear();
		}

		public static void Debug(string msg) => Print(LogLevel.Debug, msg);
		public static void Note(string msg) => Print(LogLevel.Note, msg);
		public static void Warning(string msg) => Print(LogLevel.Warning, msg);
		public static void Error(string msg) => Print(LogLevel.Error, msg);
		public static void Failure(string msg) => Print(LogLevel.Failure, msg);

		public static void Print(LogLevel level, string msg)
		{
			if(msg != null)
			{
				foreach(var pair in loggers)
				{
					if(level >= pair.Value)
					{ pair.Key.Print(level, msg); }
				}
			}
			else
			{ Log.Error("couldn't print message to log: message is null"); }
		}
	};
}