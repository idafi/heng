namespace heng.Video
{
	/// <summary>
	/// Represents an engine-managed window.
	/// <para>Direct drawing to the window (through methods like <see cref="DrawPoint"/>) will not be
	/// reflected on-screen until you <see cref="Present"/> the window.</para>
	/// </summary>
	public class Window
	{
		readonly Core.Video.Windows.WindowInfo info;
		
		/// <summary>
		/// The window's ID.
		/// <para>This is an index into the <see cref="VideoState"/>'s <see cref="VideoState.Windows"/> collection.</para>
		/// </summary>
		public int ID => info.ID;

		/// <summary>
		/// The window's title.
		/// </summary>
		public string Title => info.Title;
		
		/// <summary>
		/// The index of the system display to which the window belongs.
		/// </summary>
		public int DisplayIndex => info.DisplayIndex;

		/// <summary>
		/// The refresh rate of the system display to which the window belongs.
		/// </summary>
		public int RefreshRate => info.RefreshRate;
		
		/// <summary>
		/// The dimensions of the system display to which the window belongs.
		/// </summary>
		public ScreenRect DisplayRect => info.DisplayRect;

		/// <summary>
		/// The dimensions of the window itself.
		/// </summary>
		public ScreenRect WindowRect => info.WindowRect;

		/// <summary>
		/// The dimensions of the viewport inside the window.
		/// </summary>
		public ScreenRect ViewportRect => info.ViewportRect;
		
		/// <summary>
		/// Closes the window.
		/// </summary>
		public void Close()
		{
			Core.Video.Windows.CloseWindow(ID);
		}
		
		/// <summary>
		/// Checks that the window is open and valid.
		/// </summary>
		/// <returns>True if the window is open and valid; false if not.</returns>
		public bool Check()
		{
			return Core.Video.Windows.CheckWindow(ID);
		}
		
		/// <summary>
		/// Clears the entire window, setting all pixels to the given color.
		/// </summary>
		/// <param name="color">The color used to clear the window.</param>
		public void Clear(Color color)
		{
			Core.Video.Windows.SetWindowColor(ID, color);
			Core.Video.Windows.ClearWindow(ID);
		}

		/// <summary>
		/// Finalizes and presents all draw operations to the window since the last call to Present.
		/// </summary>
		public void Present()
		{
			Core.Video.Windows.PresentWindow(ID);
		}

		/// <summary>
		/// Draws a one-pixel point to the window.
		/// </summary>
		/// <param name="point">The pixel position at which to draw the point.</param>
		/// <param name="color">The color of the point.</param>
		public void DrawPoint(ScreenPoint point, Color color)
		{
			Core.Video.Windows.SetWindowColor(ID, color);
			Core.Video.Windows.DrawPoint(ID, point);
		}

		/// <summary>
		/// Draws a pixel-width line to the window.
		/// </summary>
		/// <param name="line">The line to draw.</param>
		/// <param name="color">The color of the line.</param>
		public void DrawLine(ScreenLine line, Color color)
		{
			Core.Video.Windows.SetWindowColor(ID, color);
			Core.Video.Windows.DrawLine(ID, line);
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
			Core.Video.Windows.SetWindowColor(ID, color);
			Core.Video.Windows.DrawPoints(ID, Core.Video.Windows.PointsDrawMode.Points, points, points.Length);
		}

		internal Window(Core.Video.Windows.WindowInfo info)
		{
			this.info = info;
		}
	};
}