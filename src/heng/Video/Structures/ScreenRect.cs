namespace heng.Video
{
	/// <summary>
	/// Represents an integer-based, 2-dimensional rectangular pixel area.
	/// <para>Unlike a plain <see cref="Rect"/>, a <see cref="ScreenRect"/> originates
	/// from its bottom-left corner, to better parallel other screen-space operations.</para>
	/// Casting between a <see cref="Rect"/> and a <see cref="ScreenRect"/> will automatically
	/// account for the difference.
	/// </summary>
	public struct ScreenRect
	{
		/// <summary>
		/// The X coordinate of the rect's position (bottom-left corner).
		/// </summary>
		public readonly int X;

		/// <summary>
		/// The Y coordinate of the rect's position (bottom-left corner).
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
		/// Returns the rect's position (bottom-left corner) as a <see cref="ScreenPoint"/>.
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

		/// <inheritdoc />
		public static bool operator ==(ScreenRect a, ScreenRect b)
		{
			return (a.X == b.X && a.Y == b.Y && a.W == b.W && a.H == b.H);
		}

		/// <inheritdoc />
		public static bool operator !=(ScreenRect a, ScreenRect b)
		{
			return (a.X == b.X && a.Y == b.Y && a.W == b.W && a.H == b.H);
		}

		/// <inheritdoc />
		public static explicit operator Rect(ScreenRect rect)
		{
			Vector2 ext = new Vector2(rect.W / 2, rect.H / 2);
			Vector2 pos = new Vector2(rect.X, rect.Y) + ext;

			return new Rect(pos, ext);
		}

		/// <inheritdoc />
		public static explicit operator ScreenRect(Rect rect)
		{
			Vector2 pos = rect.BottomLeft;

			int x = HMath.RoundToInt(pos.X);
			int y = HMath.RoundToInt(pos.Y);
			int w = HMath.RoundToInt(rect.Extents.X * 2);
			int h = HMath.RoundToInt(rect.Extents.Y * 2);

			return new ScreenRect(x, y, w, h);
		}

		/// <inheritdoc />
		public override bool Equals(object obj) => base.Equals(obj);

		/// <inheritdoc />
		public override int GetHashCode() => base.GetHashCode();
	};
}