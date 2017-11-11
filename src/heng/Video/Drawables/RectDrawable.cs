namespace heng.Video
{
	/// <summary>
	/// A world-space-positioned <see cref="heng.Rect"/> that can be drawn to a <see cref="Window"/>.
	/// </summary>
	public class RectDrawable : IDrawable
	{
		/// <summary>
		/// The <see cref="heng.Rect"/> to draw.
		/// </summary>
		public readonly Rect Rect;

		/// <summary>
		/// The world-space position of the <see cref="heng.Rect"/>.
		/// <para>The <see cref="heng.Rect"/>'s <see cref="Rect.Center"/> will be interpreted as
		/// a pixel-space offset from this position.</para>
		/// </summary>
		public readonly WorldPoint Position;
		
		/// <summary>
		/// Whether to fill the rect.
		/// </summary>
		public readonly bool Fill;

		/// <summary>
		/// The color of the rect.
		/// </summary>
		public readonly Color Color;
		
		/// <summary>
		/// Constructs a new <see cref="RectDrawable"/>.
		/// </summary>
		/// <param name="rect">The <see cref="heng.Rect"/> to draw.</param>
		/// <param name="position">The world-space position of the <see cref="heng.Rect"/>.</param>
		/// <param name="fill">Whether to fill the rect.</param>
		/// <param name="color">The color of the rect.</param>
		public RectDrawable(Rect rect, WorldPoint position, bool fill, Color color)
		{
			Rect = rect;
			Position = position;
			Fill = fill;
			Color = color;
		}

		/// <inheritdoc />
		public void Draw(Window window, Camera camera)
		{
			ScreenPoint pos = camera.WorldToViewportPosition(Position.PixelTranslate(Rect.BottomLeft));
			int w = HMath.RoundToInt(Rect.Extents.X * 2);
			int h = HMath.RoundToInt(Rect.Extents.Y * 2);

			ScreenRect rect = new ScreenRect(pos.X, pos.Y, w, h);
			window.DrawRect(rect, Fill, Color);
		}
	};
}