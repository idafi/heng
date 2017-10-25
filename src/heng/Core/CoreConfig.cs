using System.Runtime.InteropServices;
using heng.Logging;

namespace heng
{
	/// <summary>
	/// Configuration settings for the <see cref="Engine"/>.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CoreConfig
	{
		/// <summary>
		/// Configuration settings for the engine core's logging system.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct LogConfig
		{
			/// <summary>
			/// The engine core won't print messages less severe than this to the console.
			/// </summary>
			public LogLevel MinLevelConsole;

			/// <summary>
			/// The engine core won't print messages less severe than this to the output file.
			/// </summary>
			public LogLevel MinLevelFile;
		};

		/// <summary>
		/// Configuration settings for the engine core's logging system.
		/// </summary>
		public LogConfig Log;
	};
}