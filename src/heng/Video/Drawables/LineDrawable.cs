namespace heng.Video
{
	/// <summary>
	/// A world-space line that can be drawn to a <see cref="Window"/>.
	/// </summary>
	public class LineDrawable : IDrawable
	{
		/// <summary>
		/// The line's world-space start point.
		/// </summary>
		public readonly WorldPoint Start;

		/// <summary>
		/// The line's world-space end point.
		/// </summary>
		public readonly WorldPoint End;

		/// <summary>
		/// The color of the line.
		/// </summary>
		public readonly Color Color;
		
		/// <summary>
		/// Constructs a new <see cref="LineDrawable"/>.
		/// </summary>
		/// <param name="start">The new line's world-space start point.</param>
		/// <param name="end">The new line's world-space end point.</param>
		/// <param name="color">The new line's color.</param>
		public LineDrawable(WorldPoint start, WorldPoint end, Color color)
		{
			Start = start;
			End = end;
			Color = color;
		}

		/// <inheritdoc />
		public void Draw(Window window, Camera camera)
		{
			ScreenPoint pixelStart = camera.WorldToViewportPosition(Start);
			ScreenPoint pixelEnd = camera.WorldToViewportPosition(End);
			
			window.DrawLine(new ScreenLine(pixelStart, pixelEnd), Color);
		}
	};
}