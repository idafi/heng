namespace heng.Video
{
	/// <summary>
	/// A world-space positioned <see cref="heng.Polygon"/> that can be drawn to a <see cref="Window"/>.
	/// </summary>
	public class PolygonDrawable : IDrawable
	{
		/// <summary>
		/// The <see cref="heng.Polygon"/> to draw.
		/// </summary>
		public readonly Polygon Polygon;

		/// <summary>
		/// The world-space origin of the polygon.
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// The color of the polygon.
		/// </summary>
		public readonly Color Color;

		/// <summary>
		/// Contructs a new <see cref="PolygonDrawable"/>.
		/// </summary>
		/// <param name="polygon">The <see cref="heng.Polygon"/> to draw.</param>
		/// <param name="position">The world-space origin of the polygon.</param>
		/// <param name="color">The color of the polygon.</param>
		public PolygonDrawable(Polygon polygon, WorldPoint position, Color color)
		{
			Polygon = polygon;
			Position = position;
			Color = color;
		}

		/// <inheritdoc />
		public void Draw(Window window)
		{
			ScreenPoint[] points = new ScreenPoint[Polygon.Points.Count];
			for(int i = 0; i < points.Length; i++)
			{ points[i] = (ScreenPoint)(Polygon.Points[i] + Position.PixelPosition); }

			window.DrawPolygon(points, Color);
		}
	};
}