using System.Collections.Generic;

namespace heng
{
	/// <summary>
	/// Represents a 2-dimensional pixel-space polygon.
	/// </summary>
	public struct Polygon
	{
		readonly Vector2[] points;
		
		/// <summary>
		/// The points comprising the <see cref="Polygon"/>.
		/// </summary>
		public IReadOnlyList<Vector2> Points => points;
		
		/// <summary>
		/// Constructs a new <see cref="Polygon"/> using the given points.
		/// </summary>
		/// <param name="points">The points with which to construct the new <see cref="Polygon"/>.</param>
		public Polygon(params Vector2[] points)
		{
			this.points = points;
		}

		/// <summary>
		/// Constructs a new <see cref="Polygon"/> using the given points.
		/// </summary>
		/// <param name="points">The points with which to construct the new <see cref="Polygon"/>.</param>
		public Polygon(IEnumerable<Vector2> points)
		{
			this.points = new List<Vector2>(points).ToArray();
		}
		
		/// <summary>
		/// Translates the <see cref="Polygon"/> by the given translation vector.
		/// </summary>
		/// <param name="translation">The vector by which to translate the <see cref="Polygon"/></param>
		/// <returns>A new, translated <see cref="Polygon"/>.</returns>
		public Polygon Translate(Vector2 translation)
		{
			return new Polygon(TranslatePoints(translation));
		}

		IEnumerable<Vector2> TranslatePoints(Vector2 translation)
		{
			foreach(Vector2 point in points)
			{ yield return point + translation; }
		}
	};
}