namespace heng.Video
{
	/// <summary>
	/// A world-space point that can be drawn to a <see cref="Window"/>.
	/// </summary>
	public class PointDrawable : IDrawable
	{
		/// <summary>
		/// The point's world-space position.
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// The color of the point.
		/// </summary>
		public readonly Color Color;
		
		/// <summary>
		/// Constructs a new <see cref="PointDrawable"/>.
		/// </summary>
		/// <param name="position">The new point's world-space position.</param>
		/// <param name="color">The new line's color.</param>
		public PointDrawable(WorldPoint position, Color color)
		{
			Position = position;
			Color = color;
		}

		/// <inheritdoc />
		public void Draw(Window window, Camera camera)
		{
			ScreenPoint pixelPos = camera.WorldToViewportPosition(Position);
			
			window.DrawPoint(pixelPos, Color);
		}
	};
}