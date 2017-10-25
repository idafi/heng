namespace heng
{
	/// <summary>
	/// Entry point for engine initialization and shutdown.
	/// <para>Most heng systems require the engine to be properly initialized; expect errors if you
	/// try to use them without calling <see cref="Init(CoreConfig)"/>.</para>
	/// While the engine will try to clean up as gracefully as possible when the program exits, you should
	/// still make sure to call <see cref="Quit"/> on shutdown, even on emergency failure.
	/// </summary>
	public static class Engine
	{
		/// <summary>
		/// Initializes the engine and its services, using the provided configuration structure.
		/// <para>Duplicate calls shouldn't have adverse effects, but try to avoid them anyway.</para>
		/// </summary>
		/// <param name="config">Configuration settings for engine services.</param>
		/// <returns>True if initialization was successful; false if not.</returns>
		public static bool Init(CoreConfig config) => Core.Init(config);

		/// <summary>
		/// Shuts down the engine and its services.
		/// <para>Duplicate calls shouldn't have adverse effects, but try to avoid them anyway.</para>
		/// </summary>
		public static void Quit() => Core.Quit();
	};
}