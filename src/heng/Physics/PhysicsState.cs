using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// Represents an immutable snapshot of the physics system's current state.
	/// <para>Each frame, the provided <see cref="RigidBody"/> and <see cref="StaticBody"/> instances
	/// apply any forces they've accumulated, before testing and resolving collisions.</para>
	/// (This is a really pathetic and egregiously inaccurate representation of "physics," but it works for now.)
	/// </summary>
	public class PhysicsState
	{
		/// <summary>
		/// All <see cref="RigidBody"/> instances in use.
		/// </summary>
		public readonly IReadOnlyList<RigidBody> RigidBodies;

		/// <summary>
		/// All <see cref="StaticBody"/> instances in use.
		/// </summary>
		public readonly IReadOnlyList<StaticBody> StaticBodies;

		/// <summary>
		/// Constructs a new <see cref="PhysicsState"/> using the provided physics objects.
		/// <para>All physics-related translations and calculations will take place during this construction.</para>
		/// E.g., the <see cref="RigidBody"/> instances that will be present in the <see cref="RigidBodies"/>
		/// collection will be in a new positon, after all impulses are applied and all collisions are resolved.
		/// </summary>
		/// <param name="rigidBodies"></param>
		/// <param name="staticBodies"></param>
		public PhysicsState(IEnumerable<RigidBody> rigidBodies, IEnumerable<StaticBody> staticBodies)
		{
			List<RigidBody> newRigidBodies = new List<RigidBody>();
			List<StaticBody> newStaticBodies = new List<StaticBody>(staticBodies);

			foreach(RigidBody b in rigidBodies)
			{ newRigidBodies.Add(b.ApplyImpulses()); }

			List<IPhysicsObject> allObjects = new List<IPhysicsObject>();
			allObjects.AddRange(newRigidBodies);
			allObjects.AddRange(newStaticBodies);

			CollisionTester tester = new CollisionTester();
			foreach(CollisionData collision in tester.GetCollisions(allObjects))
			{
				if(collision.Object is RigidBody rb)
				{
					// FIXME: slow FIXME FIXME slow FIXME slow slow slow slow slow FIXME
					int i = newRigidBodies.FindIndex(r => r == rb);
					newRigidBodies[i] = rb.Translate(collision.MTV);
				}
			}

			RigidBodies = newRigidBodies;
			StaticBodies = newStaticBodies;
		}
	};
}