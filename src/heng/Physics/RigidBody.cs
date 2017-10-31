namespace heng.Physics
{
	/// <summary>
	/// Represents a physics object that can accumulate and respond to impulse forces.
	/// <para>Rather than directly translating the object, movement should be effected through
	/// applying impulses to the <see cref="RigidBody"/> via <see cref="AddImpulse(Vector2)"/>.</para>
	/// Accumulated impulses will then be resolved when the <see cref="PhysicsState"/> is constructed.
	/// </summary>
	public class RigidBody : IPhysicsObject
	{
		/// <summary>
		/// The pixel-space position at which this <see cref="RigidBody"/> is located.
		/// </summary>
		public readonly Vector2 Position;

		/// <summary>
		/// The collidable representation of this <see cref="RigidBody"/>.
		/// <para>If null, the <see cref="RigidBody"/> can still move, but won't respond to collisions.</para>
		/// </summary>
		public readonly ICollider Collider;

		/// <summary>
		/// The total accumulation of all impules forces to be applied on <see cref="PhysicsState"/> construction.
		/// </summary>
		public readonly Vector2 TotalImpulse;

		/// <inheritdoc />
		Vector2 IPhysicsObject.Position => Position;

		/// <inheritdoc />
		ICollider IPhysicsObject.Collider => Collider;
	
		/// <summary>
		/// Constructs a new <see cref="RigidBody"/> at the provided position, using the provided collider shape.
		/// </summary>
		/// <param name="position">The position at which to place the new <see cref="RigidBody"/>.</param>
		/// <param name="collider">The collidable representation of the new <see cref="RigidBody"/>.
		/// <para>This can be null if the <see cref="RigidBody"/> doesn't need to respond to collisions.</para></param>
		public RigidBody(Vector2 position, ICollider collider)
		{
			Position = position;
			Collider = collider;

			TotalImpulse = Vector2.Zero;
		}
		
		RigidBody(Vector2 position, ICollider collider, Vector2 totalImpulse)
		{
			Position = position;
			Collider = collider;

			TotalImpulse = totalImpulse;
		}
		
		/// <summary>
		/// Directly translates the <see cref="RigidBody"/> by the given movement vector.
		/// <para>Avoid this whenever possible. Add impulses through <see cref="AddImpulse(Vector2)"/> instead.</para>
		/// </summary>
		/// <param name="translation">The movement vector by which to translate the <see cref="RigidBody"/>.</param>
		/// <returns>A new <see cref="RigidBody"/>, translated by the movement vector.</returns>
		public RigidBody Translate(Vector2 translation) => new RigidBody(Position + translation, Collider, TotalImpulse);

		/// <summary>
		/// Adds an impulse force to this <see cref="RigidBody"/>.
		/// <para>Impulses will be cumulatively applied when constructing a new <see cref="PhysicsState"/>
		/// with this <see cref="RigidBody"/>.</para>
		/// </summary>
		/// <param name="impulse">The impulse force to add to the <see cref="RigidBody"/>.</param>
		/// <returns>A new <see cref="RigidBody"/>, whose total accumlated impulse includes the new force.</returns>
		public RigidBody AddImpulse(Vector2 impulse) => new RigidBody(Position, Collider, TotalImpulse + impulse);
		
		/// <summary>
		/// Applies all accumulated impulses to the <see cref="RigidBody"/>, translating it by
		/// the total impulse vector.
		/// <para>This is automatically invoked when the <see cref="PhysicsState"/> is constructed.
		/// You should never have to do it.</para>
		/// (in the current pathetic not-"physics" representation, the total impulse is cleared each update,
		/// rather than added to a persistent velocity.)
		/// </summary>
		/// <returns>A new <see cref="RigidBody"/>, translated by its accumulated <see cref="TotalImpulse"/>.</returns>
		public RigidBody ApplyImpulses() => new RigidBody(Position + TotalImpulse, Collider, Vector2.Zero);
	};
}