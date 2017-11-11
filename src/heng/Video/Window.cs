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
			if(id > -1)
			{
				if(title == null)
				{
					Log.Warning("new Window has null title");
					title = "";
				}

				ID = id;
				Title = title;

				Rect = rect;
				WindowFlags = windowFlags;
				RendererFlags = rendererFlags;
			}
			else
			{ Log.Error("couldn't construct new Window: ID is negative"); }
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
			if(old != null)
			{
				ID = old.ID;
				Title = title ?? old.Title;
				Rect = rect ?? old.Rect;
				WindowFlags = windowFlags ?? old.WindowFlags;
				RendererFlags = rendererFlags ?? old.RendererFlags;
			}
			else
			{ Log.Error("couldn't construct new Window: old Window is null"); }
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
			if(points == null)
			{
				Log.Warning("tried to draw null array of ScreenPoints to window " + ID);
				points = new ScreenPoint[0];
			}

			Core.Video.Queue.DrawPoints(ID, color, points, points.Length);
		}

		/// <summary>
		/// Draws a polygon to the window, whose vertices consist of the given points.
		/// </summary>
		/// <param name="points">The vertices of the polygon.</param>
		/// <param name="color">The color of the polygon.</param>
		public void DrawPolygon(ScreenPoint[] points, Color color)
		{
			if(points == null)
			{
				Log.Warning("tried to draw null array of ScreenPoints to window " + ID);
				points = new ScreenPoint[0];
			}

			Core.Video.Queue.DrawPolygon(ID, color, points, points.Length);
		}

		/// <summary>
		/// Draws a rect to the window, optionally filling it.
		/// </summary>
		/// <param name="rect">The rect to draw.</param>
		/// <param name="fill">Whether to fill the rect with the given color.</param>
		/// <param name="color">The color of the rect and, if applicable, its fill.</param>
		public void DrawRect(ScreenRect rect, bool fill, Color color)
		{
			Core.Video.Queue.DrawRect(ID, color, rect, fill);
		}

		/// <summary>
		/// Draws a <see cref="Texture"/> to the window.
		/// </summary>
		/// <param name="tex">The <see cref="Texture"/> to draw.</param>
		/// <param name="position">The position at which to draw the <see cref="Texture"/>.</param>
		/// <param name="rotation">The number of degrees the <see cref="Texture"/> should be rotated before drawn.</param>
		public void DrawTexture(Texture tex, ScreenPoint position, float rotation)
		{
			if(tex != null)
			{ tex.Draw(this.ID, position, rotation); }
			else
			{ Log.Warning("tried to draw null Sprite to window " + ID); }
		}
	};
}