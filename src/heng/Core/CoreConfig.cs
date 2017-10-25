using System.Runtime.InteropServices;

namespace heng
{
	[StructLayout(LayoutKind.Sequential)]
	public struct CoreConfig
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct LogConfig
		{
			public LogLevel MinLevelConsole;
			public LogLevel MinLevelFile;
		};

		public LogConfig Log;
	};
}