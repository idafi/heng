namespace heng.Physics
{
	/// <summary>
	/// Represents a physics object that never moves.
	/// <para>A <see cref="StaticBody"/> provides an unmoving collider that <see cref="RigidBody"/>
	/// instances can collide against.</para>
	/// You could construct a <see cref="StaticBody"/> with no collider, but that would be pointless.
	/// </summary>
	public class StaticBody : IPhysicsObject
	{
		/// <summary>
		/// The position at which the <see cref="StaticBody"/> is located.
		/// </summary>
		public readonly Vector2 Position;

		/// <summary>
		/// The collidable representation of this <see cref="StaticBody"/>.
		/// </summary>
		public readonly ICollider Collider;

		Vector2 IPhysicsObject.Position => Position;
		ICollider IPhysicsObject.Collider => Collider;
	
		/// <summary>
		/// Constructs a new <see cref="StaticBody"/> at the given position, using the given <see cref="ICollider"/>.
		/// </summary>
		/// <param name="position">The position at which to construct the new <see cref="StaticBody"/>.</param>
		/// <param name="collider">The collidable representation of the new <see cref="StaticBody"/>.</param>
		public StaticBody(Vector2 position, ICollider collider)
		{
			Position = position;
			Collider = collider;
		}
	};
}