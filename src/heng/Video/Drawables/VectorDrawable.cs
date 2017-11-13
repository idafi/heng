namespace heng.Video
{
	/// <summary>
	/// Draws a <see cref="Vector2"/> originating at a world-space position.
	/// </summary>
	public class VectorDrawable : IDrawable
	{
		/// <summary>
		/// The <see cref="Vector2"/> to draw.
		/// </summary>
		public readonly Vector2 Vector;

		/// <summary>
		/// The origin of the vector.
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// The color of the vector.
		/// </summary>
		public readonly Color Color;

		/// <summary>
		/// Constructs a new <see cref="VectorDrawable"/>.
		/// </summary>
		/// <param name="vector">The <see cref="Vector2"/> to draw.</param>
		/// <param name="position">The origin of the vector.</param>
		/// <param name="color">The color of the vector.</param>
		public VectorDrawable(Vector2 vector, WorldPoint position, Color color)
		{
			Vector = vector;
			Position = position;
			Color = color;
		}

		/// <inheritdoc />
		public void Draw(Window window, Camera camera)
		{
			ScreenPoint start = camera.WorldToViewportPosition(Position);
			ScreenPoint end = start + (ScreenPoint)(Vector);

			window.DrawLine(new ScreenLine(start, end), Color);
		}
	};
}