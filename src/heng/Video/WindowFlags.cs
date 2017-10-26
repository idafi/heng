using System;

namespace heng.Video
{
	/// <summary>
	/// Configuration flags for engine windows.
	/// <para>This is just a direct translation of SDL2's video flags - don't mess with them.</para>
	/// </summary>
	[Flags]
	public enum WindowFlags : UInt32
	{
		/// <summary>
		/// Fullscreen window.
		/// </summary>
		Fullscreen =	0x00000001,

		/// <summary>
		/// Window usable with OpenGL context.
		/// </summary>
		OpenGL =		0x00000002,

		/// <summary>
		/// Window is visible.
		/// </summary>
		Shown =			0x00000004,

		/// <summary>
		/// Window is not visible.
		/// </summary>
		Hidden =		0x00000008,

		/// <summary>
		/// No window decoration.
		/// </summary>
		Borderless =	0x00000010,

		/// <summary>
		/// Window can be resized.
		/// </summary>
		Resizable =		0x00000020,
		
		/// <summary>
		/// Window is minimzed.
		/// </summary>
		Minimized =		0x00000040,

		/// <summary>
		/// Window is maximized.
		/// </summary>
		Maximized =		0x00000080,

		/// <summary>
		/// Window has grabbed input focus.
		/// </summary>
		InputGrabbed =	0x00000100,

		/// <summary>
		/// Window has input focus.
		/// </summary>
		InputFocus =	0x00000200,

		/// <summary>
		/// Window has mouse focus.
		/// </summary>
		MouseFocus =	0x00000400,

		/// <summary>
		/// Window is fullscreen, at the current desktop resolution.
		/// </summary>
		FullscreenDesktop = ( Fullscreen | 0x00001000 ),

		/// <summary>
		/// Window is not created by the engine.
		/// </summary>
		Foreign =		0x00000800,

		/// <summary>
		/// Window should be created in high-DPI mode, if supported.
		/// </summary>
		AllowHighDPI =	0x00002000,

		/// <summary>
		/// Window has mouse captured (unrelated to <see cref="InputGrabbed"/>.)
		/// </summary>
		MouseCapture =	0x00004000,

		/// <summary>
		/// Window should always be above others.
		/// </summary>
		AlwaysOnTop =	0x00008000,

		/// <summary>
		/// Window should not be added to the taskbar.
		/// </summary>
		SkipTaskbar =	0x00010000,

		/// <summary>
		/// Window should be treated as a utility window.
		/// </summary>
		Utility =		0x00020000,

		/// <summary>
		/// Window should be treated as a tooltip.
		/// </summary>
		Tooltip =		0x00040000,

		/// <summary>
		/// Window should be treated as a popup menu.
		/// </summary>
		PopupMenu =		0x00080000
	};
}