namespace heng.Video
{
	/// <summary>
	/// Represents an integer-based, 2-dimensional pixel position.
	/// </summary>
	public struct ScreenPoint
	{
		/// <summary>
		/// The X coordinate of this point.
		/// </summary>
		public readonly int X;

		/// <summary>
		/// The Y coordinate of this point.
		/// </summary>
		public readonly int Y;
	
		/// <summary>
		/// Constructs a new <see cref="ScreenPoint"/> using the given X and Y coordinates.
		/// </summary>
		/// <param name="x">The X coordinate of the new point.</param>
		/// <param name="y">The Y coordinate of the new point.</param>
		public ScreenPoint(int x, int y)
		{
			X = x;
			Y = y;
		}
	
		/// <summary>
		/// Returns a new position at the sum of two <see cref="ScreenPoint"/>s' coodinates.
		/// </summary>
		/// <param name="a">The first <see cref="ScreenPoint"/> to add.</param>
		/// <param name="b">The second <see cref="ScreenPoint"/> to add.</param>
		/// <returns>A new <see cref="ScreenPoint"/>, whose X and Y coordinates sum those of the two operands.</returns>
		public static ScreenPoint operator +(ScreenPoint a, ScreenPoint b) => new ScreenPoint(a.X + b.X, a.Y + b.Y);

		/// <summary>
		/// Returns the difference between two <see cref="ScreenPoint"/>s.
		/// </summary>
		/// <param name="a">The <see cref="ScreenPoint"/> to subtract from.</param>
		/// <param name="b">The <see cref="ScreenPoint"/> to subtract with.</param>
		/// <returns>A new <see cref="ScreenPoint"/>, whose X and Y coordinates represent the difference between the two operands.</returns>
		public static ScreenPoint operator -(ScreenPoint a, ScreenPoint b) => new ScreenPoint(a.X - b.X, a.Y - b.Y);

		/// <summary>
		/// Tests if two <see cref="ScreenPoint"/>s represent the same position.
		/// </summary>
		/// <param name="a">The first <see cref="ScreenPoint"/> to test.</param>
		/// <param name="b">The second <see cref="ScreenPoint"/> to test.</param>
		/// <returns>True if the <see cref="ScreenPoint"/>s represent the same position; false if not.</returns>
		public static bool operator ==(ScreenPoint a, ScreenPoint b) => (a.X == b.X && a.Y == b.Y);

		/// <summary>
		/// Tests if two <see cref="ScreenPoint"/>s do not represent the same position.
		/// </summary>
		/// <param name="a">The first <see cref="ScreenPoint"/> to test.</param>
		/// <param name="b">The second <see cref="ScreenPoint"/> to test.</param>
		/// <returns>True if the <see cref="ScreenPoint"/>s do not represent the same positio; false if they do.</returns>
		public static bool operator !=(ScreenPoint a, ScreenPoint b) => (a.X != b.X || a.Y != b.Y);
	
		/// <inheritdoc />
		public override bool Equals(object obj) => base.Equals(obj);

		/// <inheritdoc />
		public override int GetHashCode() => base.GetHashCode();

		/// <inheritdoc />
		public override string ToString() => $"({X}, {Y})";
	};
}