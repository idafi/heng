using System.Collections.Generic;
using heng;
using heng.Physics;

namespace hgame
{
	public partial class GamestateBuilder
	{
		public class PhysicsStateBuilder
		{
			readonly List<RigidBody> rigidBodies;
			readonly List<StaticBody> staticBodies;

			public PhysicsStateBuilder()
			{
				rigidBodies = new List<RigidBody>();
				staticBodies = new List<StaticBody>();
			}

			public int AddRigidBody(RigidBody body)
			{
				Assert.Ref(body);

				rigidBodies.Add(body);
				return rigidBodies.Count - 1;
			}

			public int AddStaticBody(StaticBody body)
			{
				Assert.Ref(body);

				staticBodies.Add(body);
				return staticBodies.Count - 1;
			}

			public PhysicsState Build()
			{
				return new PhysicsState(rigidBodies, staticBodies);
			}

			public void Clear()
			{
				rigidBodies.Clear();
				staticBodies.Clear();
			}
		};
	};
}