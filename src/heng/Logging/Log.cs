using System.Collections.Generic;
using heng.Logging;

namespace heng
{
	/// <summary>
	/// Provides globally-available logging systems.
	/// <para>An arbitrary number of log destinations are supported via the <see cref="ILogger"/> interface.</para>
	/// <see cref="ILogger"/>s are registered with a minimum <see cref="LogLevel"/> - messages print only if the message
	/// level matches or exceeds the logger's minimum level.
	/// </summary>
	public static class Log
	{
		static readonly Dictionary<ILogger, LogLevel> loggers;

		static Log() => loggers = new Dictionary<ILogger, LogLevel>();

		/// <summary>
		/// Adds the given <see cref="ILogger"/> to the logging system.
		/// </summary>
		/// <param name="logger">The <see cref="ILogger"/> to add.</param>
		/// <param name="minLevel">The minimum <see cref="LogLevel"/> required for messages to print to this <see cref="ILogger"/>.</param>
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

		/// <summary>
		/// Removes the given <see cref="ILogger"/> from the logging system.
		/// </summary>
		/// <param name="logger">The <see cref="ILogger"/> to remove.</param>
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

		/// <summary>
		/// Removes all <see cref="ILogger"/>s from the logging system.
		/// </summary>
		public static void ClearLoggers()
		{
			loggers.Clear();
		}

		/// <summary>
		/// Shortcut for logging a message at <see cref="LogLevel.Debug"/>.
		/// </summary>
		/// <param name="msg">The message to log.</param>
		public static void Debug(string msg) => Print(LogLevel.Debug, msg);

		/// <summary>
		/// Shortcut for logging a message at <see cref="LogLevel.Note"/>.
		/// </summary>
		/// <param name="msg">The message to log.</param>
		public static void Note(string msg) => Print(LogLevel.Note, msg);

		/// <summary>
		/// Shortcut for logging a message at <see cref="LogLevel.Warning"/>.
		/// </summary>
		/// <param name="msg">The message to log.</param>
		public static void Warning(string msg) => Print(LogLevel.Warning, msg);

		/// <summary>
		/// Shortcut for logging a message at <see cref="LogLevel.Error"/>.
		/// </summary>
		/// <param name="msg">The message to log.</param>
		public static void Error(string msg) => Print(LogLevel.Error, msg);

		/// <summary>
		/// Shortcut for logging a message at <see cref="LogLevel.Failure"/>.
		/// </summary>
		/// <param name="msg">The message to log.</param>
		public static void Failure(string msg) => Print(LogLevel.Failure, msg);

		/// <summary>
		/// Logs a message to all registered <see cref="ILogger"/>s, providing it meets their minimum <see cref="LogLevel"/>.
		/// </summary>
		/// <param name="level">The level at which the message should be logged.</param>
		/// <param name="msg">The message string.</param>
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