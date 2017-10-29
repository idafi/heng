using System.Collections.Generic;

namespace heng.Physics
{
	public interface ICollider
	{
		IEnumerable<Vector2> GetSeperatingAxes(ICollider other);
		ColliderProjection Project(Vector2 offset, Vector2 axis);
	};
}