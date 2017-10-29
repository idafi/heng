using System.Collections.Generic;

namespace heng.Physics
{
	public class PhysicsState
	{
		public readonly IReadOnlyList<RigidBody> RigidBodies;
		public readonly IReadOnlyList<StaticBody> StaticBodies;

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