namespace heng.Video
{
	/// <summary>
	/// Represents an engine-managed window.
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
		
		internal Window(Core.Video.Windows.WindowInfo info)
		{
			this.info = info;
		}
	};
}