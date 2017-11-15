using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// Represents a world-space object that can respond to physical forces.
	/// <para>An <see cref="IPhysicsBody"/>'s behavior is defined by an impulse pass and a
	/// collision pass.</para>
	/// The impulse pass applies accumulated forces; the collision pass resolves collisions
	/// that result from the impulse pass.
	/// </summary>
	public interface IPhysicsBody
	{
		/// <summary>
		/// The world-space position of the <see cref="IPhysicsBody"/>.
		/// </summary>
		WorldPoint Position { get;}

		/// <summary>
		/// The collidable representation of the <see cref="IPhysicsBody"/>.
		/// <para>A null value should be interpreted as a non-colliding <see cref="IPhysicsBody"/>.</para>
		/// </summary>
		ICollider Collider { get; }

		/// <summary>
		/// The total mass of the <see cref="IPhysicsBody"/>.
		/// <para>Higher mass values require more force to affect velocity.</para>
		/// </summary>
		float Mass { get; }

		/// <summary>
		/// The <see cref="PhysicsMaterial"/> representing the <see cref="IPhysicsBody"/>.
		/// <para><see cref="PhysicsMaterial"/>s describe how the <see cref="IPhysicsBody"/> respond to collisions.</para>
		/// </summary>
		PhysicsMaterial Material { get; }

		/// <summary>
		/// The current pixel-space velocity of the <see cref="IPhysicsBody"/>.
		/// </summary>
		Vector2 Velocity { get; }

		/// <summary>
		/// Applies accumulated forces to the <see cref="IPhysicsBody"/>.
		/// </summary>
		/// <param name="gravity">The gravity vector present in the simulation.</param>
		/// <param name="deltaT">Seconds passed since the last physics update.</param>
		/// <returns>A new <see cref="IPhysicsBody"/>, with its accumulated forces applied.</returns>
		IPhysicsBody ImpulsePass(Vector2 gravity, float deltaT);

		/// <summary>
		/// Resolves collisions between this <see cref="IPhysicsBody"/> and other bodies.
		/// </summary>
		/// <param name="collisions">All collisions by this <see cref="IPhysicsBody"/>.</param>
		/// <returns>A new <see cref="IPhysicsBody"/>, with all collisions resolved.</returns>
		IPhysicsBody CollisionPass(IEnumerable<CollisionData> collisions);
	};
}