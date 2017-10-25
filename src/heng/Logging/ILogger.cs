namespace heng.Logging
{
	/// <summary>
	/// Describes an output destination for the <see cref="Log"/> system.
	/// <para>Messages will not be sent to <see cref="Print(LogLevel, string)"/> if their <see cref="LogLevel"/>
	/// does not match or exceed the minimum level specified by <see cref="Log.AddLogger(ILogger, LogLevel)"/>.</para>
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Prints a message string to an output destination.
		/// </summary>
		/// <param name="level">The message's <see cref="LogLevel"/> severity.</param>
		/// <param name="msg">The message string.</param>
		void Print(LogLevel level, string msg);
	};
}