using System;
using System.Runtime.InteropServices;

namespace heng
{
	internal static partial class Core
	{
		public static class Time
		{
			[StructLayout(LayoutKind.Sequential)]
			public struct State
			{
				public readonly UInt32 TotalTicks;
				public readonly UInt32 DeltaTicks;

				public readonly UInt32 TargetFrametime;
			};

			[DllImport(coreLib, EntryPoint = "Time_Delay")]
			public static extern void Delay(UInt32 ms);

			[DllImport(coreLib, EntryPoint = "Time_SetTargetFrametime")]
			public static extern void SetTargetFrametime(UInt32 ms);

			[DllImport(coreLib, EntryPoint = "Time_DelayToTarget")]
			public static extern void DelayToTarget();

			[DllImport(coreLib, EntryPoint = "Time_TimeProcedure")]
			public static extern void TimeProcedure(Action proc);

			[DllImport(coreLib, EntryPoint = "Time_GetSnapshot")]
			public static extern void GetSnapshot(out Time.State state);
		};
	};
}