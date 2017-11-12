using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// Represents an immutable snapshot of the physics system's current state.
	/// <para>Each frame, the provided <see cref="IPhysicsObject"/> instances apply
	/// any forces they've accumulated, before testing and resolving collisions.</para>
	/// </summary>
	public class PhysicsState
	{
		/// <summary>
		/// All <see cref="IPhysicsObject"/> instances in use.
		/// </summary>
		public readonly IReadOnlyList<IPhysicsObject> PhysicsObjects;

		/// <summary>
		/// Constructs a new <see cref="PhysicsState"/> using the provided physics objects.
		/// <para>During construction, <see cref="RigidBody"/> forces will be applied, and collisions will be resolved.</para>
		/// An accurate delta-time value is needed to ensure even movement across variable framerates.
		/// </summary>
		/// <param name="physicsObjects">All <see cref="IPhysicsObject"/>s available to the new state.</param>
		/// <param name="deltaT">The time, in seconds, since the last <see cref="PhysicsState"/> was built.</param>
		public PhysicsState(IEnumerable<IPhysicsObject> physicsObjects, float deltaT)
		{
			PhysicsObjects = AddPhysicsObjects(physicsObjects, deltaT);
		}

		IReadOnlyList<IPhysicsObject> AddPhysicsObjects(IEnumerable<IPhysicsObject> objects, float deltaT)
		{
			if(objects != null)
			{
				List<IPhysicsObject> newObjects = ImpulsePass(objects, deltaT);
				IReadOnlyList<IPhysicsObject> finalObjects = CollisionPass(newObjects);

				return finalObjects;
			}
			else
			{ Log.Warning("PhysicsState constructed will null physics objects collection"); }

			return new IPhysicsObject[0];
		}

		List<IPhysicsObject> ImpulsePass(IEnumerable<IPhysicsObject> objects, float deltaT)
		{
			Assert.Ref(objects);

			List<IPhysicsObject> newObjects = new List<IPhysicsObject>();

			foreach(IPhysicsObject obj in objects)
			{
				if(obj != null)
				{
					if(obj is RigidBody rb)
					{ newObjects.Add(rb.Update(deltaT)); }
					else
					{ newObjects.Add(obj); }
				}
				else
				{ Log.Error("couldn't add IPhysicsObject to PhysicsState: object is null"); }
			}

			return newObjects;
		}

		IReadOnlyList<IPhysicsObject> CollisionPass(List<IPhysicsObject> newObjects)
		{
			Assert.Ref(newObjects);

			CollisionTester tester = new CollisionTester();
			foreach(CollisionData collision in tester.GetCollisions(newObjects))
			{
				if(collision.Object is RigidBody rb)
				{
					// FIXME: slow FIXME FIXME slow FIXME slow slow slow slow slow FIXME
					int i = newObjects.FindIndex(r => r == rb);
					newObjects[i] = rb.Translate(collision.MTV);
				}
			}

			return newObjects;
		}
	};
}