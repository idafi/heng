using System.Collections.Generic;
using heng;
using heng.Physics;

namespace hgame
{
	public partial class GamestateBuilder
	{
		public class PhysicsStateBuilder
		{
			readonly List<IPhysicsBody> physicsBodies;
			Vector2 gravity;

			public PhysicsStateBuilder()
			{
				physicsBodies = new List<IPhysicsBody>();
				gravity = new Vector2(0, -550);
			}

			public int AddPhysicsObject(IPhysicsBody obj)
			{
				Assert.Ref(obj);

				physicsBodies.Add(obj);
				return physicsBodies.Count - 1;
			}

			public void SetGravity(Vector2 gravity)
			{
				this.gravity = gravity;
			}

			public PhysicsState Build(float deltaT)
			{
				return new PhysicsState(physicsBodies, gravity, deltaT);
			}

			public void Clear()
			{
				physicsBodies.Clear();
			}
		};
	};
}