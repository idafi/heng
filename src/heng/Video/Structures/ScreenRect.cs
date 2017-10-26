namespace heng.Video
{
	/// <summary>
	/// Represents an integer-based, 2-dimensional pixel area.
	/// </summary>
	public struct ScreenRect
	{
		/// <summary>
		/// The X coordinate of the rect's position.
		/// </summary>
		public readonly int X;

		/// <summary>
		/// The Y coordinate of the rect's position.
		/// </summary>
		public readonly int Y;

		/// <summary>
		/// The width of the rect.
		/// </summary>
		public readonly int W;

		/// <summary>
		/// The height of the rect.
		/// </summary>
		public readonly int H;

		/// <summary>
		/// Returns the rect's position as a <see cref="ScreenPoint"/>.
		/// </summary>
		public ScreenPoint Position => new ScreenPoint(X, Y);

		/// <summary>
		/// Constructs a new <see cref="ScreenRect"/> with the given components.
		/// </summary>
		/// <param name="x">The X coordinate of the new rect's position.</param>
		/// <param name="y">The Y coordinate of the new rect's position.</param>
		/// <param name="w">The width of the new rect.</param>
		/// <param name="h">The height of the new rect.</param>
		public ScreenRect(int x, int y, int w, int h)
		{
			X = x;
			Y = y;
			W = w;
			H = h;
		}
	};
}