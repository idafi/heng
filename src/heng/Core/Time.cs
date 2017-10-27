using System;
using System.Runtime.InteropServices;

namespace heng
{
	internal static partial class Core
	{
		public static class Time
		{
			[DllImport(coreLib, EntryPoint = "Time_GetTicks")]
			public static extern UInt32 GetTicks();

			[DllImport(coreLib, EntryPoint = "Time_Delay")]
			public static extern void Delay(UInt32 ms);

			[DllImport(coreLib, EntryPoint = "Time_TimeProcedure")]
			public static extern void TimeProcedure(Action proc);
		};
	};
}