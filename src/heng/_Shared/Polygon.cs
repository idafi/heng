using System.Collections.Generic;

namespace heng
{
	public struct Polygon
	{
		readonly Vector2[] points;
		
		public IReadOnlyList<Vector2> Points => points;
		
		public Polygon(params Vector2[] points)
		{
			this.points = points;
		}

		public Polygon(IEnumerable<Vector2> points)
		{
			this.points = new List<Vector2>(points).ToArray();
		}
		
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