namespace heng.Logging
{
	/// <summary>
	/// Describes the severity of a <see cref="Log"/> message.
	/// <para>The <see cref="Log"/> system routes messages only to the <see cref="ILogger"/>s
	/// which accept the given <see cref="LogLevel"/>.</para>
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Extra fine-grain information, relevant only for debugging.
		/// </summary>
		Debug,

		/// <summary>
		/// A helpful notice that things are behaving as expected.
		/// </summary>
		Note,

		/// <summary>
		/// Something weird happened, but without adverse effects.
		/// </summary>
		Warning,

		/// <summary>
		/// Something bad happened, leading to one or more adverse or unexpected effects.
		/// </summary>
		Error,

		/// <summary>
		/// Something really bad happened, and the program needs to shut down before something worse happens.
		/// </summary>
		Failure
	};
}