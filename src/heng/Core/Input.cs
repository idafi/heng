using System;
using System.Runtime.InteropServices;
using heng.Input;

namespace heng
{
	internal static partial class Core
	{
		public static class Input
		{
			public const int ControllersMax = 4;
			
			[StructLayout(LayoutKind.Sequential)]
			public struct State
			{
				[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = (int)(KeyCode.Count))]
				public readonly bool[] Keyboard;

				public readonly MouseState Mouse;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = ControllersMax)]
				public readonly ControllerState[] Controllers;
			};

			[StructLayout(LayoutKind.Sequential)]
			public struct MouseState
			{
				public readonly int X;
				public readonly int Y;

				[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = (int)(MouseButtonCode.Count))]
				public readonly bool[] Buttons;
			};

			[StructLayout(LayoutKind.Sequential)]
			public struct ControllerState
			{
				[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = (int)(ControllerButtonCode.Count))]
				public readonly bool[] Buttons;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)(AxisCode.Count))]
				public readonly Int16[] Axes;
			};

			[DllImport(coreLib, EntryPoint = "Input_GetSnapshot")]
			public static extern void GetSnapshot(out Input.State state);
		};
	};
}