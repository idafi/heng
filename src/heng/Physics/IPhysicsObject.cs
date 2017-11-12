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
	};
}