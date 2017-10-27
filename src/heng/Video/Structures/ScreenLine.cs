namespace heng.Video
{
	/// <summary>
	/// Represents a pixel line drawn between two integer-based <see cref="ScreenPoint"/>s.
	/// </summary>
	public struct ScreenLine
	{
		/// <summary>
		/// The <see cref="ScreenPoint"/> at the start of the line.
		/// </summary>
		public readonly ScreenPoint Start;
		
		/// <summary>
		/// The <see cref="ScreenPoint"/> at the end of the line.
		/// </summary>
		public readonly ScreenPoint End;
		
		/// <summary>
		/// Constructs a new <see cref="ScreenLine"/> using the given <see cref="ScreenPoint"/>s.
		/// </summary>
		/// <param name="start">The <see cref="ScreenPoint"/> at the start of the new line.</param>
		/// <param name="end">The <see cref="ScreenPoint"/> at the end of the new line.</param>
		public ScreenLine(ScreenPoint start, ScreenPoint end)
		{
			Start = start;
			End = end;
		}

		/// <inheritdoc />
		public static bool operator ==(ScreenLine a, ScreenLine b) => (a.Start == b.Start) && (a.End == b.End);

		/// <inheritdoc />
		public static bool operator !=(ScreenLine a, ScreenLine b) => (a.Start != b.Start) || (a.End != b.End);

		/// <inheritdoc />
		public override bool Equals(object obj) => base.Equals(obj);

		/// <inheritdoc />
		public override int GetHashCode() => base.GetHashCode();
	};
}