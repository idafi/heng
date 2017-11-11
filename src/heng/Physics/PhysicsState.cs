using System.Collections.Generic;

namespace heng.Physics
{
	/// <summary>
	/// Represents an immutable snapshot of the physics system's current state.
	/// <para>Each frame, the provided <see cref="IPhysicsObject"/> instances apply
	/// any forces they've accumulated, before testing and resolving collisions.</para>
	/// (This is a really pathetic and egregiously inaccurate representation of "physics," but it works for now.)
	/// </summary>
	public class PhysicsState
	{
		/// <summary>
		/// All <see cref="IPhysicsObject"/> instances in use.
		/// </summary>
		public readonly IReadOnlyList<IPhysicsObject> PhysicsObjects;

		/// <summary>
		/// Constructs a new <see cref="PhysicsState"/> using the provided physics objects.
		/// <para>All physics-related transformations and calculations will take place during this construction.</para>
		/// E.g., the <see cref="RigidBody"/> instances that will be present in the <see cref="PhysicsObjects"/>
		/// collection will be in a new positon, after all impulses are applied and all collisions are resolved.
		/// </summary>
		public PhysicsState(IEnumerable<IPhysicsObject> physicsObjects)
		{
			PhysicsObjects = AddPhysicsObjects(physicsObjects);
		}

		IReadOnlyList<IPhysicsObject> AddPhysicsObjects(IEnumerable<IPhysicsObject> objects)
		{
			if(objects != null)
			{
				List<IPhysicsObject> newObjects = ImpulsePass(objects);
				IReadOnlyList<IPhysicsObject> finalObjects = CollisionPass(newObjects);

				return finalObjects;
			}
			else
			{ Log.Warning("PhysicsState constructed will null physics objects collection"); }

			return new IPhysicsObject[0];
		}

		List<IPhysicsObject> ImpulsePass(IEnumerable<IPhysicsObject> objects)
		{
			Assert.Ref(objects);

			List<IPhysicsObject> newObjects = new List<IPhysicsObject>();

			foreach(IPhysicsObject obj in objects)
			{
				if(obj != null)
				{
					if(obj is RigidBody rb)
					{ newObjects.Add(rb.ApplyImpulses()); }
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