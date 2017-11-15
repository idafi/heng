namespace heng.Physics
{
	/// <summary>
	/// Describes collision properties of an <see cref="IPhysicsBody"/>.
	/// </summary>
	public class PhysicsMaterial
	{
		/// <summary>
		/// The amount of force needed to change a non-moving object's velocity.
		/// </summary>
		public readonly float StaticFriction;

		/// <summary>
		/// The amount of force exerted against a moving object.
		/// </summary>
		public readonly float KineticFriction;

		/// <summary>
		/// The amount of kinetic energy retained on collision ("bounciness").
		/// <para>A value of 1 preserves all kinetic energy (perpetual bouncing); a value of 0 indicates
		/// all kinetic energy is lost (full stop - no bouncing).</para>
		/// </summary>
		public readonly float Restitution;

		/// <summary>
		/// Constructs a new <see cref="PhysicsMaterial"/>.
		/// </summary>
		/// <param name="staticFriction">The amount of force needed to change a non-moving object's velocity.</param>
		/// <param name="kineticFriction">The amount of force exerted against a moving object.</param>
		/// <param name="restitution">The amount of kinetic energy retained on collision ("bounciness").</param>
		public PhysicsMaterial(float staticFriction, float kineticFriction, float restitution)
		{
			StaticFriction = staticFriction;
			KineticFriction = kineticFriction;
			Restitution = restitution;
		}
	};
}