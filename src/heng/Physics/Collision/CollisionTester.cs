using System.Collections.Generic;

namespace heng.Physics
{
	internal class CollisionTester
	{
		public IEnumerable<CollisionData> GetCollisions(IReadOnlyList<IPhysicsObject> objects)
		{
			// don't test if there's nothing to test
			if(objects.Count > 1)
			{
				// we're just checking each collider against those at larger indices
				// (i.e., those which it hasn't yet been checked against)
				for(int a = 0; a < objects.Count - 1; a++)
				{
					IPhysicsObject oa = objects[a];

					for(int b = a + 1; b < objects.Count; b++)
					{
						IPhysicsObject ob = objects[b];

						if(TestPair(oa, ob, out Vector2 mtv))
						{
							yield return new CollisionData(oa, mtv);
							yield return new CollisionData(ob, -mtv);
						}
					}
				}
			}
		}

		bool TestPair(IPhysicsObject a, IPhysicsObject b, out Vector2 mtv)
		{
			// we're also returning the minimum translation vector
			float minOverlap = float.MaxValue;
			mtv = Vector2.Zero;

			// test seperating axes
			foreach(Vector2 axis in GetAllSeperatingAxes(a.Collider, b.Collider))
			{
				// project colliders on this seperating axis
				ColliderProjection projA = a.Collider.Project(a.Position, axis);
				ColliderProjection projB = b.Collider.Project(b.Position, axis);

				// find the overlap. if there is none, we're not colliding and can get out now
				float overlap = ColliderProjection.GetOverlap(projA, projB);
				if(overlap > 0)
				{
					// otherwise, update the mtv if it's a smaller overlap than before
					if(overlap < minOverlap)
					{
						minOverlap = overlap;
						mtv = axis;
					}
				}
				else
				{ return false; }
			}

			// make sure mtv isn't negative
			Vector2 diff = a.Position - b.Position;
			if(diff.Dot(mtv) < 0)
			{ mtv = -mtv; }

			// scale mtv by the smallest overlap, and we're done
			mtv *= minOverlap;
			return true;
		}
		
		IEnumerable<Vector2> GetAllSeperatingAxes(ICollider a, ICollider b)
		{
			foreach(Vector2 v in a.GetSeperatingAxes())
			{ yield return v; }
			foreach(Vector2 v in b.GetSeperatingAxes())
			{ yield return v; }
		}
	};
}