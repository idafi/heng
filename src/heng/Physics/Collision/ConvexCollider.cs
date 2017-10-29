using System.Collections.Generic;

namespace heng.Physics
{
	public class ConvexCollider : ICollider
	{
		public readonly Polygon Shape;

		public IEnumerable<Vector2> GetSeperatingAxes(ICollider other)
		{
			// TODO: handle single-point shapes
			if(Shape.Points.Count > 1)
			{
				Vector2 prev = Shape.Points[Shape.Points.Count - 1];
				foreach(Vector2 p in Shape.Points)
				{
					// seperating axis is simply the normal of each edge
					Vector2 current = p;
					Vector2 edge = current - prev;
					edge = edge.Normalize().LeftNormal;

					// remember: SAT is early-out; no point in returning all axes if one is found not to overlap
					yield return edge;
					prev = current;
				}
			}
		}

		public ColliderProjection Project(Vector2 offset, Vector2 axis)
		{
			// move shape into whatever local space the collision checker is wanting
			Polygon shape = Shape.Translate(offset);

			float min = float.MaxValue;
			float max = float.MinValue;

			foreach(Vector2 p in shape.Points)
			{
				// 1d projection of 2d shape is the dot product of each point along the projection target
				float dot = p.Dot(axis);

				// but we're only interested in the min/max points of the projection
				if(dot > max)
				{ max = dot; }
				if(dot < min)
				{ min = dot; }
			}

			return new ColliderProjection(min, max);
		}

		public ConvexCollider(Polygon shape)
		{
			Shape = shape;
		}
	};
}