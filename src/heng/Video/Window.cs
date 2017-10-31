namespace heng.Video
{
	/// <summary>
	/// Represents an engine-managed window, for use by a <see cref="VideoState"/> instance.
	/// <para>Each frame, you should use construct new <see cref="Window"/>s, draw what needs to be drawn,
	/// and use those <see cref="Window"/>s to construct a new <see cref="VideoState"/> instance for the frame.</para>
	/// The underlying engine then uses each <see cref="Window"/>'s <see cref="ID"/> to automatically handle
	/// OS-level window management.
	/// </summary>
	public class Window
	{
		/// <summary>
		/// Use this magic SDL2 constant in the window's <see cref="Rect"/> to specify a centered window coordinate.
		/// <para>E.g., setting the rect's position to (<see cref="Center"/>, <see cref="Center"/>) will position the
		/// window in the center of the screen.</para>
		/// </summary>
		public const int Center = 805240832;

		/// <summary>
		/// Unique ID of this window.
		/// <para>The engine uses window IDs to manage opening and closing actual OS windows. If a window
		/// with the same ID already exists when a new <see cref="VideoState"/> is constructed, 
		/// the OS window will simply be modified to reflect the new <see cref="Window"/> instance's
		/// settings, rather than literally closing and reconstructing the OS window anew.</para>
		/// </summary>
		public readonly int ID;

		/// <summary>
		/// Title text of this window.
		/// </summary>
		public readonly string Title;

		/// <summary>
		/// Dimensions of this window.
		/// </summary>
		public readonly ScreenRect Rect;

		/// <summary>
		/// Configuration settings of this window.
		/// </summary>
		public readonly WindowFlags WindowFlags;

		/// <summary>
		/// Configuration settings of this window's renderer.
		/// </summary>
		public readonly RendererFlags RendererFlags;

		/// <summary>
		/// Constructs a new Window instance.
		/// </summary>
		/// <param name="id">The unique ID of the window.</param>
		/// <param name="title">The title of the window.</param>
		/// <param name="rect">The dimensions of the window.</param>
		/// <param name="windowFlags">Configuration settings for the window.</param>
		/// <param name="rendererFlags">Configuration settings for the window's renderer.</param>
		public Window(int id, string title, ScreenRect rect, WindowFlags windowFlags, RendererFlags rendererFlags)
		{
			ID = id;
			Title = title;

			Rect = rect;
			WindowFlags = windowFlags;
			RendererFlags = rendererFlags;
		}

		/// <summary>
		/// Constructs a new Window instance, based on an old instance.
		/// <para>The window's ID is always kept, but any other parameters set to null
		/// will carry settings over from the old instance.</para>
		/// </summary>
		/// <param name="old">The old instance upon which the new Window is based.</param>
		/// <param name="title">The title of the window.</param>
		/// <param name="rect">The dimensions of the window.</param>
		/// <param name="windowFlags">Configuration settings for the window.</param>
		/// <param name="rendererFlags">Configuration settings for the window's renderer.</param>
		public Window(Window old, string title = null, ScreenRect? rect = null,
			WindowFlags? windowFlags = null, RendererFlags? rendererFlags = null)
		{
			ID = old.ID;
			Title = title ?? old.Title;
			Rect = rect ?? old.Rect;
			WindowFlags = windowFlags ?? old.WindowFlags;
			RendererFlags = rendererFlags ?? old.RendererFlags;
		}

		/// <summary>
		/// Clears the window, setting each pixel to the given color.
		/// </summary>
		/// <param name="color">The color used to clear the <see cref="Window"/>.</param>
		public void Clear(Color color)
		{
			Core.Video.Queue.ClearWindow(ID, color);
		}

		/// <summary>
		/// Draws a one-pixel point to the window.
		/// </summary>
		/// <param name="point">The pixel position at which to draw the point.</param>
		/// <param name="color">The color of the point.</param>
		public void DrawPoint(ScreenPoint point, Color color)
		{
			Core.Video.Queue.DrawPoint(ID, color, point);
		}

		/// <summary>
		/// Draws a pixel-width line to the window.
		/// </summary>
		/// <param name="line">The line to draw.</param>
		/// <param name="color">The color of the line.</param>
		public void DrawLine(ScreenLine line, Color color)
		{
			Core.Video.Queue.DrawLine(ID, color, line);
		}

		/// <summary>
		/// Draws multiple one-pixel points to the window.
		/// <para>There is no practical distinction between multiple calls to <see cref="DrawPoint"/>,
		/// beyond being more efficient if drawing large numbers of points at once.</para>
		/// </summary>
		/// <param name="points">The array of points to draw.</param>
		/// <param name="color">The color of the points.</param>
		public void DrawPoints(ScreenPoint[] points, Color color)
		{
			Core.Video.Queue.DrawPoints(ID, color, points, points.Length);
		}

		/// <summary>
		/// Draws a texture to the window.
		/// </summary>
		/// <param name="textureID">The ID of the texture.</param>
		/// <param name="position">The position at which to draw the texture.</param>
		/// <param name="rotation">The rotation, in degrees, with which to draw the texture.</param>
		public void DrawTexture(int textureID, ScreenPoint position, float rotation)
		{
			Core.Video.Queue.DrawTexture(ID, textureID, position, rotation);
		}

		/// <summary>
		/// Draws a <see cref="Sprite"/> to the window.
		/// <para>This is functionally equivalent to calling <see cref="Sprite"/>.<see cref="Sprite.Draw"/>
		/// with this <see cref="Window"/> as an argument.</para>
		/// </summary>
		/// <param name="spr">The <see cref="Sprite"/> to draw.</param>
		public void DrawSprite(Sprite spr)
		{
			spr.Draw(this);
		}
	};
}