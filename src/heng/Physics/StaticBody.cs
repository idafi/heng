using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// An infinite-mass <see cref="IPhysicsBody"/> that never moves.
	/// <para>A <see cref="StaticBody"/> can't accumulate forces, and will never be affected
	/// by collisions. (They are consequently useless without a collider.)</para>
	/// Why bother? A <see cref="StaticBody"/> is much less taxing on the physics simulation than a <see cref="RigidBody"/>.
	/// </summary>
	public class StaticBody : IPhysicsBody
	{
		/// <summary>
		/// The world-space position of the <see cref="StaticBody"/>.
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// The collidable representation of the <see cref="StaticBody"/>.
		/// <para>If null, the <see cref="StaticBody"/> can't do anything, and is effectively useless.</para>
		/// </summary>
		public readonly ICollider Collider;

		/// <summary>
		/// The <see cref="PhysicsMaterial"/> representing the <see cref="StaticBody"/>.
		/// </summary>
		public readonly PhysicsMaterial Material;

		WorldPoint IPhysicsBody.Position => Position;
		ICollider IPhysicsBody.Collider => Collider;
		float IPhysicsBody.Mass => float.PositiveInfinity;
		PhysicsMaterial IPhysicsBody.Material => Material;
		Vector2 IPhysicsBody.Velocity => Vector2.Zero;

		/// <summary>
		/// Constructs a new <see cref="StaticBody"/>.
		/// </summary>
		/// <param name="position">The world-space position of the new <see cref="StaticBody"/>.</param>
		/// <param name="collider">The collidable representation of the new <see cref="StaticBody"/>.</param>
		/// <param name="material">The <see cref="PhysicsMaterial"/> representing the new <see cref="StaticBody"/>.</param>
		public StaticBody(WorldPoint position, ICollider collider, PhysicsMaterial material)
		{
			if(collider == null)
			{ Log.Warning("constructed a StaticBody with no collider"); }

			Position = position;
			Collider = collider;

			Material = material;
		}

		IPhysicsBody IPhysicsBody.ImpulsePass(Vector2 gravity, float deltaT) => this;
		IPhysicsBody IPhysicsBody.CollisionPass(IEnumerable<CollisionData> collisions) => this;
	};
}