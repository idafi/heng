using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// Represents an immutable snapshot of the physics system's current state.
	/// <para>Each frame, <see cref="IPhysicsBody"/> instances are placed in the simulation,
	/// applying any forces they've accumulated, and resolving any resulting collisions.</para>
	/// </summary>
	public class PhysicsState
	{
		/// <summary>
		/// All <see cref="IPhysicsBody"/> instances being simulated.
		/// </summary>
		public readonly IReadOnlyList<IPhysicsBody> PhysicsBodies;

		/// <summary>
		/// The overall gravity force applied to the simulation.
		/// </summary>
		public readonly Vector2 Gravity;

		/// <summary>
		/// Constructs a new snapshot of the physics system's state.
		/// </summary>
		/// <param name="physicsBodies">All <see cref="IPhysicsBody"/> instances to be simulated.</param>
		/// <param name="gravity">The total force of gravity applied to the simulation.</param>
		/// <param name="deltaT">Seconds since the previous <see cref="PhysicsState"/> was constructed.</param>
		public PhysicsState(IEnumerable<IPhysicsBody> physicsBodies, Vector2 gravity, float deltaT)
		{
			Gravity = gravity;
			PhysicsBodies = AddBodies(physicsBodies, deltaT);
		}

		IReadOnlyList<IPhysicsBody> AddBodies(IEnumerable<IPhysicsBody> bodies, float deltaT)
		{
			if(bodies != null)
			{
				IReadOnlyList<IPhysicsBody> newBodies = ImpulsePass(bodies, deltaT);
				newBodies = CollisionPass(newBodies);

				return newBodies;
			}

			return new IPhysicsBody[0];
		}

		IReadOnlyList<IPhysicsBody> ImpulsePass(IEnumerable<IPhysicsBody> bodies, float deltaT)
		{
			List<IPhysicsBody> newBodies = new List<IPhysicsBody>();

			foreach(IPhysicsBody body in bodies)
			{
				if(body != null)
				{ newBodies.Add(body.ImpulsePass(Gravity, deltaT)); }
			}

			return newBodies;
		}

		IReadOnlyList<IPhysicsBody> CollisionPass(IReadOnlyList<IPhysicsBody> bodies)
		{
			List<IPhysicsBody> newBodies = new List<IPhysicsBody>();

			CollisionTester tester = new CollisionTester();
			var collisions = tester.GetCollisions(bodies);

			foreach(IPhysicsBody body in bodies)
			{
				IPhysicsBody newBody = body;
				if(collisions.TryGetValue(body, out IReadOnlyList<CollisionData> bodyCollisions))
				{ newBody = body.CollisionPass(bodyCollisions); }

				newBodies.Add(newBody);
			}

			return newBodies;
		}
	};
}