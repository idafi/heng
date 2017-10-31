using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// A collidable shape representing an <see cref="IPhysicsObject"/>.
	/// <para>Collisions are tested using the seperating axis theorem; the <see cref="ICollider"/>
	/// provides the per-collider data necessary to detect collisions via SAT.</para>
	/// </summary>
	public interface ICollider
	{
		/// <summary>
		/// Gets the normal of each collider edge as a normalized axis vector.
		/// </summary>
		/// <returns>Normalized axis vectors representing each collider edge.</returns>
		IEnumerable<Vector2> GetSeperatingAxes();

		/// <summary>
		/// Projects the collider at the given pixel-space position, on the given 1-dimensional axis.
		/// </summary>
		/// <param name="position">The pixel-space position at which the collidable shape is located.</param>
		/// <param name="axis">The 1-dimensional axis on which to project the collider.</param>
		/// <returns>A <see cref="ColliderProjection"/> containing the min and max bounds of the projection.</returns>
		ColliderProjection Project(Vector2 position, Vector2 axis);
	};
}