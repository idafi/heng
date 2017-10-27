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
				public readonly Textures.State Textures;
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

				public enum PointsDrawMode
				{
					Points,
					Lines
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

				[DllImport(coreLib, EntryPoint = "Video_Windows_SetWindowColor")]
				public static extern void SetWindowColor(int windowID, Color color);

				[DllImport(coreLib, EntryPoint = "Video_Windows_ClearWindow")]
				public static extern void ClearWindow(int windowID);

				[DllImport(coreLib, EntryPoint = "Video_Windows_PresentWindow")]
				public static extern void PresentWindow(int windowID);

				[DllImport(coreLib, EntryPoint = "Video_Windows_DrawPoint")]
				public static extern void DrawPoint(int windowID, ScreenPoint point);

				[DllImport(coreLib, EntryPoint = "Video_Windows_DrawLine")]
				public static extern void DrawLine(int windowID, ScreenLine line);

				[DllImport(coreLib, EntryPoint = "Video_Windows_DrawPoints")]
				public static extern void DrawPoints(int windowID, PointsDrawMode mode, ScreenPoint[] points, int count);

				[DllImport(coreLib, EntryPoint = "Video_Windows_DrawTexture")]
				public static extern void DrawTexture(int windowID, int textureID, ScreenPoint position, float rotation);
			};

			public static class Textures
			{
				[StructLayout(LayoutKind.Sequential)]
				public struct State
				{
					public readonly int MaxSurfaces;
					public readonly int SurfaceCount;

					public readonly int CacheSize;
					public readonly int CacheUsage;
				};

				[DllImport(coreLib, EntryPoint = "Video_Textures_LoadTexture")]
				public static extern int LoadTexture(string filePath);

				[DllImport(coreLib, EntryPoint = "Video_Textures_FreeTexture")]
				public static extern void FreeTexture(int textureID);

				[DllImport(coreLib, EntryPoint = "Video_Textures_CheckTexture")]
				[return: MarshalAs(UnmanagedType.U1)]
				public static extern bool CheckTexture(int textureID);

				[DllImport(coreLib, EntryPoint = "Video_Textures_ClearCache")]
				public static extern void ClearCache();
			};

			[DllImport(coreLib, EntryPoint = "Video_GetSnapshot")]
			public static extern void GetSnapshot(out Video.State state);
		};
	};
}