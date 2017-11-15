using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// An <see cref="IPhysicsBody"/> that can accumulate impulses and resolve collisions.
	/// <para>Rather than teleporting to new positions, a <see cref="RigidBody"/> is moved by adding
	/// impulses via <see cref="AddImpulse(Vector2)"/>.</para>
	/// After all impulses (including gravity) are applied, the <see cref="RigidBody"/> will then respond
	/// to any consequent collisions.
	/// </summary>
	public class RigidBody : IPhysicsBody
	{
		/// <summary>
		/// The world-space position of the <see cref="RigidBody"/>.
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// The collidable representation of the <see cref="RigidBody"/>.
		/// <para>If null, the <see cref="RigidBody"/> won't create or respond to collisions,
		/// but it can still receive impulses.</para>
		/// </summary>
		public readonly ICollider Collider;

		/// <summary>
		/// The total mass of the <see cref="RigidBody"/>.
		/// <para>Higher values require stronger impulses to alter velocity.</para>
		/// </summary>
		public readonly float Mass;

		/// <summary>
		/// The <see cref="PhysicsMaterial"/> representing the <see cref="RigidBody"/>.
		/// </summary>
		public readonly PhysicsMaterial Material;

		/// <summary>
		/// The current pixel-space velocity of the <see cref="RigidBody"/>.
		/// </summary>
		public readonly Vector2 Velocity;

		/// <summary>
		/// All impulses yet to be applied to the <see cref="RigidBody"/>.
		/// <para>This will be zero once the <see cref="RigidBody"/> is constructed into
		/// the <see cref="PhysicsState"/>.</para>
		/// </summary>
		public readonly Vector2 Forces;

		WorldPoint IPhysicsBody.Position => Position;
		ICollider IPhysicsBody.Collider => Collider;
		float IPhysicsBody.Mass => Mass;
		PhysicsMaterial IPhysicsBody.Material => Material;
		Vector2 IPhysicsBody.Velocity => Velocity;

		/// <summary>
		/// Constructs a new <see cref="RigidBody"/>.
		/// </summary>
		/// <param name="position">The world-space position of the new <see cref="RigidBody"/>.</param>
		/// <param name="collider">The collidable representation of the new <see cref="RigidBody"/>.</param>
		/// <param name="mass">The total mass of the new <see cref="RigidBody"/>.</param>
		/// <param name="material">The <see cref="PhysicsMaterial"/> representing the new <see cref="RigidBody"/>.</param>
		public RigidBody(WorldPoint position, ICollider collider, float mass, PhysicsMaterial material)
			: this(null, position, collider, mass, material, Vector2.Zero, Vector2.Zero) { }
		
		RigidBody(RigidBody old, WorldPoint? position = null, ICollider collider = null, float? mass = null,
			PhysicsMaterial material = null, Vector2? velocity = null, Vector2? forces = null)
		{
			if(mass <= 0)
			{
				Log.Warning("RigidBody mass must be greater than 0");
				mass = float.Epsilon;
			}

			Position = position ?? old?.Position ?? WorldPoint.Zero;
			Collider = collider ?? old?.Collider;

			Mass = mass ?? old?.Mass ?? 1;
			Material = material ?? old?.Material ?? new PhysicsMaterial(0, 0, 1);

			Velocity = velocity ?? old?.Velocity ?? Vector2.Zero;
			Forces = forces ?? old?.Forces ?? Vector2.Zero;
		}

		/// <summary>
		/// Changes the collidable representation of the <see cref="RigidBody"/>.
		/// <para>If set to null, the <see cref="RigidBody"/> will no longer create or respond to collisions.</para>
		/// </summary>
		/// <param name="collider">The new collider, or null if the <see cref="RigidBody"/> shouldn't collide.</param>
		/// <returns>A new <see cref="RigidBody"/>, with the new <see cref="ICollider"/> assigned.</returns>
		public RigidBody SetCollider(ICollider collider) => new RigidBody(this, collider: collider);

		/// <summary>
		/// Changes the mass of the <see cref="RigidBody"/>.
		/// </summary>
		/// <param name="mass">The new mass value.</param>
		/// <returns>A new <see cref="RigidBody"/>, using the new mass value.</returns>
		public RigidBody SetMass(float mass) => new RigidBody(this, mass: mass);

		/// <summary>
		/// Adds an impulse force to the <see cref="RigidBody"/>.
		/// <para>This will be applied when the <see cref="RigidBody"/> is constructed into
		/// the <see cref="PhysicsState"/>.</para>
		/// </summary>
		/// <param name="impulse">The impulse to add.</param>
		/// <returns>A new <see cref="RigidBody"/>, with the impulse force added.</returns>
		public RigidBody AddImpulse(Vector2 impulse) => new RigidBody(this, forces: Forces + impulse);

		IPhysicsBody IPhysicsBody.ImpulsePass(Vector2 gravity, float deltaT)
		{
			WorldPoint secOrigin = new WorldPoint(Position.Sector, Vector2.Zero);
			Vector2 f = Forces + gravity;

			Vector2 a = GetAccel(f, Mass);
			Vector2 p = Position.PixelDistance(secOrigin);
			Vector2 newP = GetNewPosition(p, a, Velocity, deltaT);
			Vector2 newVelocity = GetNewVelocity(a, Velocity, deltaT);

			WorldPoint newPosition = secOrigin.PixelTranslate(newP);

			return new RigidBody(this, position: newPosition, velocity: newVelocity, forces: Vector2.Zero);
		}

		IPhysicsBody IPhysicsBody.CollisionPass(IEnumerable<CollisionData> collisions)
		{
			WorldPoint newPosition = Position;
			Vector2 newVelocity = Velocity;

			foreach(CollisionData collision in collisions)
			{
				newPosition = newPosition.PixelTranslate(collision.MTV);
				newVelocity = GetMomentumChange(this, collision) + GetFriction(this, collision);
			}

			return new RigidBody(this, position: newPosition, velocity: newVelocity);
		}

		Vector2 GetAccel(Vector2 f, float m)
		{
			return f * (1 / m);
		}

		Vector2 GetNewVelocity(Vector2 a, Vector2 v, float t)
		{
			return (a * t) + v;
		}

		Vector2 GetNewPosition(Vector2 p, Vector2 a, Vector2 v, float t)
		{
			return (a * 0.5f * HMath.Square(t)) + (v * t) + p;
		}

		Vector2 GetMomentumChange(IPhysicsBody body, CollisionData collision)
		{
			// only deal with velocity along the collision normal
			Vector2 aV = collision.Normal.Project(body.Velocity);
			Vector2 bV = collision.Normal.Project(collision.Other.Velocity);

			float aM = body.Mass;
			float bM = collision.Other.Mass;

			// get net restitution coefficient (higher values means more "bouncy")
			float c = GetRestitutionCoefficient(body, collision.Other);

			Vector2 change;

			// handle infinite mass
			if(body.Mass == float.PositiveInfinity)
			{ change = Vector2.Zero; }	// this body has inifinte mass - no energy is sent back
			else if(collision.Other.Mass == float.PositiveInfinity)
			{ change = -(aV * c); }		// the other body has infinite mass - all energy not lost is sent back
			else
			{ change = ((aV * aM) + (bV * bM) + ((bV - aV) * bM * c)) * (1 / (aM + bM)); }

			// replace old velocity component with the calculated momentum-shifted velocity
			return ((body.Velocity - aV) + change);
		}

		Vector2 GetFriction(IPhysicsBody body, CollisionData collision)
		{
			// get relative momentum, use it to project normal force & calculate tangent
			Vector2 v = (body.Velocity - collision.Other.Velocity) * body.Mass;
			Vector2 fN = collision.Normal.Project(v);
			Vector2 t = v - fN;

			// get coefficient
			float c = GetFrictionCoefficient(body, collision.Other, t);

			// friction force is normalized tangent inverse * normal force magnitude * coefficient
			Vector2 fF = -(t.Normalize() * fN.Magnitude * c);
			fF = fF.ClampMagnitude(0, t.Magnitude);	// don't let the friction force exceed the tangent's magnitude

			return fF;
		}

		float GetRestitutionCoefficient(IPhysicsBody a, IPhysicsBody b)
		{
			float c = a.Material.Restitution + b.Material.Restitution;
			return (c / 2f);
		}

		float GetFrictionCoefficient(IPhysicsBody a, IPhysicsBody b, Vector2 t)
		{
			// TODO: user defined
			const float fudge = 0.002f;

			float uA, uB;

			if(t.IsApproximately(Vector2.Zero, fudge))
			{
				uA = a.Material.StaticFriction;
				uB = b.Material.StaticFriction;
			}
			else
			{
				uA = a.Material.KineticFriction;
				uB = b.Material.KineticFriction;
			}

			return (uA + uB) / 2;
		}
	};
}