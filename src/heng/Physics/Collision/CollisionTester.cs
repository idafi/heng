using System.Collections.Generic;
using System.Linq;

namespace heng.Physics
{
	internal class CollisionTester
	{
		// FIXME: this is the ugliest method ever devised and should be cleaned and improved immediately
		public IReadOnlyDictionary<IPhysicsBody, IReadOnlyList<CollisionData>> GetCollisions(IReadOnlyList<IPhysicsBody> objects)
		{
			Assert.Ref(objects);

			var grouped = from IPhysicsBody obj in objects
						  group obj by obj.Position.Sector;

			var collisions = new Dictionary<IPhysicsBody, List<CollisionData>>();

			foreach(var group in grouped)
			{
				IReadOnlyList<IPhysicsBody> g = new List<IPhysicsBody>(group);
				foreach((IPhysicsBody, CollisionData) collision in TestGroup(g))
				{
					if(!collisions.ContainsKey(collision.Item1))
					{ collisions[collision.Item1] = new List<CollisionData>(); }

					collisions[collision.Item1].Add(collision.Item2);
				}
			}

			var outDict = new Dictionary<IPhysicsBody, IReadOnlyList<CollisionData>>();
			foreach(var pair in collisions)
			{ outDict.Add(pair.Key, pair.Value); }

			return outDict;
		}

		IEnumerable<(IPhysicsBody, CollisionData)> TestGroup(IReadOnlyList<IPhysicsBody> objects)
		{
			// we're just checking each collider against those at larger indices
			// (i.e., those which it hasn't yet been checked against)
			for(int a = 0; a < objects.Count - 1; a++)
			{
				IPhysicsBody oa = objects[a];

				if(oa.Collider != null)
				{
					for(int b = a + 1; b < objects.Count; b++)
					{
						IPhysicsBody ob = objects[b];
						if(ob.Collider != null)
						{
							// don't bother checking collisions between non-moving objects
							// (this will implicitly factor out StaticBodies)
							if(oa.Velocity != Vector2.Zero || ob.Velocity != Vector2.Zero)
							{
								if(TestPair(oa, ob, out Vector2 mtv))
								{
									// return
									yield return (oa, new CollisionData(ob, mtv));
									yield return (ob, new CollisionData(oa, -mtv));
								}
							}
						}
					}
				}
			}
		}

		bool TestPair(IPhysicsBody a, IPhysicsBody b, out Vector2 mtv)
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

		Vector2 TransferMomentum(Vector2 aV, float aM, Vector2 bV, float bM, float c)
		{
			// handle infinite mass objects (i.e. StaticBody)
			if(aM == float.PositiveInfinity)
			{ return Vector2.Zero; }
			if(bM == float.PositiveInfinity)
			{ return -aV * c; }

			return (aV * aM) + (bV * bM) + ((bV - aV) * bM * c) * (1 / (aM + bM));
		}
	};
}