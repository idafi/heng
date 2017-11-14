using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// Represents an object whose world-space position can be modified by physical forces.
	/// <para>An <see cref="ICollider"/> can optionally be assigned, allowing the physics
	/// system to detect and resolve collisions between <see cref="IPhysicsObject"/>s.</para>
	/// </summary>
	public interface IPhysicsObject
	{
		/// <summary>
		/// The world-space position at which the <see cref="IPhysicsObject"/> is located.
		/// </summary>
		WorldPoint Position { get; }
		
		/// <summary>
		/// The <see cref="ICollider"/> representing this <see cref="IPhysicsObject"/>.
		/// </summary>
		ICollider Collider  { get; }

		/// <summary>
		/// The total mass of the <see cref="IPhysicsObject"/>.
		/// <para>If this is infinite (<see cref="float.PositiveInfinity"/>), the <see cref="IPhysicsObject"/>
		/// will conserve all of its momentum.</para>
		/// (This allows <see cref="StaticBody"/> objects to stay within the same overall physics simulation.)
		/// </summary>
		float Mass { get; }

		/// <summary>
		/// The current velocity of the <see cref="IPhysicsObject"/>.
		/// </summary>
		Vector2 Velocity { get; }
	};
}