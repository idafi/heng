using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// Represents a physics object that can accumulate and respond to forces.
	/// <para>Move a RigidBody by applying impulses through <see cref="AddImpulse(Vector2)"/>.</para>
	/// All applied forces will then be resolved, including collisions, when the <see cref="RigidBody"/> is
	/// is constructed alongside other <see cref="IPhysicsObject"/>s into the <see cref="PhysicsState"/>.
	/// </summary>
	public class RigidBody : IPhysicsObject
	{
		/// <summary>
		/// The <see cref="RigidBody"/>'s world-space position.
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// The <see cref="RigidBody"/>'s collidable representation.
		/// <para>If null, the <see cref="RigidBody"/> can still move, but it won't respond to collisions.</para>
		/// </summary>
		public readonly ICollider Collider;

		/// <summary>
		/// The <see cref="RigidBody"/>'s total mass, in grams.
		/// <para>The higher the mass, the more force is required to change the velocity.</para>
		/// </summary>
		public readonly float Mass;

		/// <summary>
		/// The <see cref="RigidBody"/>'s current velocity, in pixels.
		/// </summary>
		public readonly Vector2 Velocity;

		/// <summary>
		/// Forces added to the <see cref="RigidBody"/>, but not yet applied by the <see cref="PhysicsState"/>.
		/// <para>This will always be set to (0, 0) after the <see cref="RigidBody"/> is processed
		/// by the <see cref="PhysicsState"/>.</para>
		/// </summary>
		public readonly Vector2 TotalForce;

		WorldPoint IPhysicsObject.Position => Position;
		ICollider IPhysicsObject.Collider => Collider;
		float IPhysicsObject.Mass => Mass;
		Vector2 IPhysicsObject.Velocity => Velocity;

		/// <summary>
		/// Constructs a new <see cref="RigidBody"/>.
		/// </summary>
		/// <param name="position">The new <see cref="RigidBody"/>'s world-space position.</param>
		/// <param name="collider">The new <see cref="RigidBody"/>'s collidable representation.</param>
		/// <param name="mass">The new <see cref="RigidBody"/>'s mass, in grams.</param>
		public RigidBody(WorldPoint position, ICollider collider, float mass) :
			this(position, collider, mass, Vector2.Zero, Vector2.Zero) { }
		
		RigidBody(WorldPoint position, ICollider collider, float mass, Vector2 velocity, Vector2 totalForce)
		{
			if(mass <= 0)
			{
				Log.Warning("RigidBody mass must be greater than 0");
				mass = float.Epsilon;
			}

			Position = position;
			Collider = collider;
			Mass = mass;

			Velocity = velocity;
			TotalForce = totalForce;
		}
		
		/// <summary>
		/// Adds an impulse force to the <see cref="RigidBody"/>.
		/// <para>When constructed into the <see cref="PhysicsState"/>, all accumulated forces
		/// will be applied, moving the <see cref="RigidBody"/>.</para>
		/// </summary>
		/// <param name="impulse">The total impulse to apply.</param>
		/// <returns>A new <see cref="RigidBody"/>, with the impulse applied.</returns>
		public RigidBody AddImpulse(Vector2 impulse)
		{
			return new RigidBody(Position, Collider, Mass, Velocity, TotalForce + impulse);
		}
		
		/// <summary>
		/// Sets the <see cref="RigidBody"/>'s total mass.
		/// <para>The higher the mass, the more force is required to change the velocity.</para>
		/// </summary>
		/// <param name="mass">The new mass, in grams.</param>
		/// <returns>A new <see cref="RigidBody"/>, with the new mass.</returns>
		public RigidBody SetMass(float mass)
		{
			return new RigidBody(Position, Collider, mass, Velocity, TotalForce);
		}

		internal IPhysicsObject ImpulsePass(float t)
		{
			WorldPoint secOrigin = new WorldPoint(Position.Sector, Vector2.Zero);

			Vector2 a = GetAccel(TotalForce, Mass);
			Vector2 p = Position.PixelDistance(secOrigin);
			Vector2 newP = GetNewPosition(p, a, Velocity, t);
			Vector2 newVelocity = GetNewVelocity(a, Velocity, t);

			WorldPoint newPosition = secOrigin.PixelTranslate(newP);

			return new RigidBody(newPosition, Collider, Mass, newVelocity, Vector2.Zero);
		}

		internal IPhysicsObject CollisionPass(IEnumerable<CollisionData> collisions)
		{
			WorldPoint secOrigin = new WorldPoint(Position.Sector, Vector2.Zero);

			Vector2 newPosition = Position.PixelDistance(secOrigin);
			Vector2 newVelocity = Velocity;

			foreach(CollisionData collision in collisions)
			{
				newVelocity = collision.VelocityChange;
				newPosition += collision.MTV;	// compensate for collider intersection
			}

			return new RigidBody(secOrigin.PixelTranslate(newPosition), Collider, Mass, newVelocity, TotalForce);
		}

		Vector2 GetAccel(Vector2 f, float m)
		{
			return f * (1 / m);
		}

		Vector2 GetNewPosition(Vector2 p, Vector2 a, Vector2 v, float t)
		{
			return (a * 0.5f * HMath.Square(t)) + (v * t) + p;
		}

		Vector2 GetNewVelocity(Vector2 a, Vector2 v, float t)
		{
			return (a * t) + v;
		}
	};
}