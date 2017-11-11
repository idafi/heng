namespace heng
{
	/// <summary>
	/// Represents a 2-dimensional rectangular area in pixel-space.
	/// <para>A <see cref="Rect"/> originates from its center point. Its <see cref="Extents"/> from
	/// that center point then describe its dimensions.</para>
	/// </summary>
	public struct Rect
	{
		/// <summary>
		/// The pixel-space center of the rect.
		/// </summary>
		public readonly Vector2 Center;

		/// <summary>
		/// The pixel-space extents of the rect.
		/// <para>These are half of its width/height.</para>
		/// </summary>
		public readonly Vector2 Extents;

		/// <summary>
		/// The pixel-space size of the rect.
		/// <para>This is its full width/height.</para>
		/// </summary>
		public Vector2 Size => Extents * 2;

		/// <summary>
		/// The pixel-space position of the rect's left edge.
		/// </summary>
		public Vector2 Left => Center - new Vector2(Extents.X, 0);

		/// <summary>
		/// The pixel-space position of the rect's right edge.
		/// </summary>
		public Vector2 Right => Center + new Vector2(Extents.X, 0);

		/// <summary>
		/// The pixel-space position of the rect's bottom edge.
		/// </summary>
		public Vector2 Bottom => Center - new Vector2(0, Extents.Y);

		/// <summary>
		/// The pixel-space position of the rect's top edge.
		/// </summary>
		public Vector2 Top => Center + new Vector2(0, Extents.Y);

		/// <summary>
		/// The pixel-space position of the rect's bottom-left corner.
		/// </summary>
		public Vector2 BottomLeft => Center - Extents;

		/// <summary>
		/// The pixel-space position of the rect's top-left corner.
		/// </summary>
		public Vector2 TopLeft => Center + new Vector2(-Extents.X, Extents.Y);

		/// <summary>
		/// The pixel-space position of the rect's top-right corner.
		/// </summary>
		public Vector2 TopRight => Center + Extents;

		/// <summary>
		/// The pixel-space position of the rect's bottom-right corner.
		/// </summary>
		public Vector2 BottomRight => Center + new Vector2(Extents.X, -Extents.Y);

		/// <summary>
		/// Constructs a new rect.
		/// </summary>
		/// <param name="center">The center origin of the rect, in pixel-space.</param>
		/// <param name="extents">The extents of the rect, in pixel-space.</param>
		public Rect(Vector2 center, Vector2 extents)
		{
			Center = center;
			Extents = extents;
		}
	};
}