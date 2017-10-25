using System.Runtime.InteropServices;
using heng.Logging;

namespace heng
{
	internal static partial class Core
	{
		public static class Log
		{
			public static class Console
			{
				[DllImport(coreLib, EntryPoint = "Log_Console_Print")]
				public static extern void Print(LogLevel level, string msg);

				[DllImport(coreLib, EntryPoint = "Log_Console_SetMinLevel")]
				public static extern void SetMinLevel(LogLevel level);
			};

			public static class File
			{
				[DllImport(coreLib, EntryPoint = "Log_File_Print")]
				public static extern void Print(LogLevel level, string msg);

				[DllImport(coreLib, EntryPoint = "Log_File_SetMinLevel")]
				public static extern void SetMinLevel(LogLevel level);
			}
		};
	};
}