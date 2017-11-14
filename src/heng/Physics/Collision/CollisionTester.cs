using System.Collections.Generic;
using System.Linq;

namespace heng.Physics
{
	internal class CollisionTester
	{
		// FIXME: this is the ugliest method ever devised and should be cleaned and improved immediately
		public IReadOnlyDictionary<IPhysicsObject, IReadOnlyList<CollisionData>> GetCollisions(IReadOnlyList<IPhysicsObject> objects)
		{
			Assert.Ref(objects);

			var grouped = from IPhysicsObject obj in objects
						  group obj by obj.Position.Sector;

			var collisions = new Dictionary<IPhysicsObject, List<CollisionData>>();

			foreach(var group in grouped)
			{
				IReadOnlyList<IPhysicsObject> g = new List<IPhysicsObject>(group);
				foreach((IPhysicsObject, CollisionData) collision in TestGroup(g))
				{
					if(!collisions.ContainsKey(collision.Item1))
					{ collisions[collision.Item1] = new List<CollisionData>(); }

					collisions[collision.Item1].Add(collision.Item2);
				}
			}

			var outDict = new Dictionary<IPhysicsObject, IReadOnlyList<CollisionData>>();
			foreach(var pair in collisions)
			{ outDict.Add(pair.Key, pair.Value); }

			return outDict;
		}

		IEnumerable<(IPhysicsObject, CollisionData)> TestGroup(IReadOnlyList<IPhysicsObject> objects)
		{
			// we're just checking each collider against those at larger indices
			// (i.e., those which it hasn't yet been checked against)
			for(int a = 0; a < objects.Count - 1; a++)
			{
				IPhysicsObject oa = objects[a];

				if(oa.Collider != null)
				{
					for(int b = a + 1; b < objects.Count; b++)
					{
						IPhysicsObject ob = objects[b];
						if(ob.Collider != null)
						{
							if(TestPair(oa, ob, out Vector2 mtv))
							{
								// get collision normals
								Vector2 aN = mtv.Normalize();
								Vector2 bN = -aN;

								// use them to isolate other-collider-facing component of velocity
								Vector2 aV = aN.Project(oa.Velocity);
								Vector2 bV = bN.Project(ob.Velocity);

								// transfer that component's momentum
								Vector2 aT = TransferMomentum(aV, oa.Mass, bV, ob.Mass);
								Vector2 bT = TransferMomentum(bV, ob.Mass, aV, oa.Mass);

								// remove old pre-collision component, replace with newly-computed component
								aV = (oa.Velocity - aV + aT);
								bV = (ob.Velocity - bV + bT);

								// return
								yield return (oa, new CollisionData(ob, mtv, aN, aV));
								yield return (ob, new CollisionData(oa, -mtv, bN, bV));
							}
						}
					}
				}
			}
		}

		bool TestPair(IPhysicsObject a, IPhysicsObject b, out Vector2 mtv)
		{
			Assert.Ref(a, b);

			// collisions will be tested in a localized space, originating at the least common sector of both colliders
			// (i think? this is easier than setting the origin at the collider vertex closest to 0,0)
			WorldPoint origin = WorldPoint.LeastCommonSector(a.Position, b.Position);
			Vector2 posA = a.Position.PixelDistance(origin);
			Vector2 posB = b.Position.PixelDistance(origin);

			// we're also returning the minimum translation vector
			float minOverlap = float.MaxValue;
			mtv = Vector2.Zero;

			// test seperating axes
			foreach(Vector2 axis in GetAllSeperatingAxes(a.Collider, b.Collider))
			{
				// project colliders on this seperating axis
				ColliderProjection projA = a.Collider.Project(posA, axis);
				ColliderProjection projB = b.Collider.Project(posB, axis);

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
			Vector2 diff = posA - posB;
			if(diff.Dot(mtv) < 0)
			{ mtv = -mtv; }

			// scale mtv by the smallest overlap, and we're done
			mtv *= minOverlap;
			return true;
		}
		
		IEnumerable<Vector2> GetAllSeperatingAxes(ICollider a, ICollider b)
		{
			Assert.Ref(a, b);

			foreach(Vector2 v in a.GetSeperatingAxes())
			{ yield return v; }
			foreach(Vector2 v in b.GetSeperatingAxes())
			{ yield return v; }
		}

		Vector2 TransferMomentum(Vector2 aV, float aM, Vector2 bV, float bM)
		{
			// handle infinite mass objects (i.e. StaticBody)
			if(aM == float.PositiveInfinity)
			{ return Vector2.Zero; }
			if(bM == float.PositiveInfinity)
			{ return -aV; }

			return (aV * (aM - bM) + ((bV * bM) * 2)) * (1 / (aM + bM));
		}
	};
}