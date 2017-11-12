using System.Collections.Generic;
using heng;
using heng.Physics;

namespace hgame
{
	public partial class GamestateBuilder
	{
		public class PhysicsStateBuilder
		{
			readonly List<IPhysicsObject> physicsObjects;

			public PhysicsStateBuilder()
			{
				physicsObjects = new List<IPhysicsObject>();
			}

			public int AddPhysicsObject(IPhysicsObject obj)
			{
				Assert.Ref(obj);

				physicsObjects.Add(obj);
				return physicsObjects.Count - 1;
			}

			public PhysicsState Build(float deltaT)
			{
				return new PhysicsState(physicsObjects, deltaT);
			}

			public void Clear()
			{
				physicsObjects.Clear();
			}
		};
	};
}