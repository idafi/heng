using System;
using System.Runtime.InteropServices;
using heng.Video;

namespace heng
{
	internal static partial class Core
	{
		public static class Video
		{
			[StructLayout(LayoutKind.Sequential)]
			public struct State
			{
				public readonly Windows.State Windows;
			};

			public static class Windows
			{
				[StructLayout(LayoutKind.Sequential)]
				public struct State
				{
					[MarshalAs(UnmanagedType.ByValArray, SizeConst = Max)]
					public readonly WindowInfo[] WindowInfo;
				};

				[StructLayout(LayoutKind.Sequential)]
				public struct WindowInfo
				{
					public readonly int ID;

					[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TitleMax)]
					public readonly string Title;

					public readonly int DisplayIndex;
					public readonly int RefreshRate;

					public readonly ScreenRect DisplayRect;
					public readonly ScreenRect WindowRect;
					public readonly ScreenRect ViewportRect;
				};

				public const int TitleMax = 128;
				public const int Max = 4;

				[DllImport(coreLib, EntryPoint = "Video_Windows_OpenWindow")]
				public static extern int OpenWindow(string title, ScreenRect rect, UInt32 windowFlags, UInt32 rendererFlags);

				[DllImport(coreLib, EntryPoint = "Video_Windows_CloseWindow")]
				public static extern void CloseWindow(int windowID);

				[DllImport(coreLib, EntryPoint = "Video_Windows_CheckWindow")]
				[return: MarshalAs(UnmanagedType.U1)]
				public static extern bool CheckWindow(int windowID);
			};

			[DllImport(coreLib, EntryPoint = "Video_GetSnapshot")]
			public static extern void GetSnapshot(out Video.State state);
		};
	};
}