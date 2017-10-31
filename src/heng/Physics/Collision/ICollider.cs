using System.Collections.Generic;

namespace heng.Physics
{
	public interface ICollider
	{
		IEnumerable<Vector2> GetSeperatingAxes();
		ColliderProjection Project(Vector2 position, Vector2 axis);
	};
}