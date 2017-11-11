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
				public readonly Queue.State Queue;
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
				public static extern void OpenWindow(int windowID, string title, ScreenRect rect, UInt32 windowFlags, UInt32 rendererFlags);

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

				[DllImport(coreLib, EntryPoint = "Video_Windows_DrawRect")]
				public static extern void DrawRect(int windowID, ScreenRect rect, bool fill);

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

			public static class Queue
			{
				public const int QueueSize = 1024;

				public enum VidCommandType : byte
				{
					Invalid,
					Open,
					Close,
					Mode,
					Clear,
					Points,
					Line,
					Polygon,
					Rect,
					Texture
				};

				[StructLayout(LayoutKind.Sequential)]
				public struct State
				{
					[MarshalAs(UnmanagedType.ByValArray, SizeConst = Windows.Max)]
					public readonly QueueInfo[] Queues;
				};

				[StructLayout(LayoutKind.Sequential)]
				public struct QueueInfo
				{
					public readonly int CommandCount;

					[MarshalAs(UnmanagedType.ByValArray, SizeConst = QueueSize)]
					public readonly VidCommandType[] Commands;
				};

				[DllImport(coreLib, EntryPoint = "Video_Queue_OpenWindow")]
				public static extern void OpenWindow(int windowID, string title, ScreenRect rect, UInt32 windowFlags, UInt32 rendererFlags);

				[DllImport(coreLib, EntryPoint = "Video_Queue_CloseWindow")]
				public static extern void CloseWindow(int windowID);

				[DllImport(coreLib, EntryPoint = "Video_Queue_ChangeWindowMode")]
				public static extern void ChangeWindowMode(int windowID, ScreenRect window, ScreenRect viewport);

				[DllImport(coreLib, EntryPoint = "Video_Queue_ClearWindow")]
				public static extern void ClearWindow(int windowID, Color color);

				[DllImport(coreLib, EntryPoint = "Video_Queue_DrawPoint")]
				public static extern void DrawPoint(int windowID, Color color, ScreenPoint point);

				[DllImport(coreLib, EntryPoint = "Video_Queue_DrawLine")]
				public static extern void DrawLine(int windowID, Color color, ScreenLine line);

				[DllImport(coreLib, EntryPoint = "Video_Queue_DrawPoints")]
				public static extern void DrawPoints(int windowID, Color color, ScreenPoint[] points, int count);

				[DllImport(coreLib, EntryPoint = "Video_Queue_DrawPolygon")]
				public static extern void DrawPolygon(int windowID, Color color, ScreenPoint[] points, int count);

				[DllImport(coreLib, EntryPoint = "Video_Queue_DrawRect")]
				public static extern void DrawRect(int windowID, Color color, ScreenRect rect, bool fill);

				[DllImport(coreLib, EntryPoint = "Video_Queue_DrawTexture")]
				public static extern void DrawTexture(int windowID, int textureID, ScreenPoint position, float rotation);

				[DllImport(coreLib, EntryPoint = "Video_Queue_Pump")]
				public static extern void Pump(int windowID);

				[DllImport(coreLib, EntryPoint = "Video_Queue_ClearQueue")]
				public static extern void ClearQueue(int windowID);

				[DllImport(coreLib, EntryPoint = "Video_Queue_PumpAll")]
				public static extern void PumpAll();

				[DllImport(coreLib, EntryPoint = "Video_Queue_ClearAll")]
				public static extern void ClearAll();
			};

			[DllImport(coreLib, EntryPoint = "Video_GetSnapshot")]
			public static extern void GetSnapshot(out Video.State state);
		};
	};
}