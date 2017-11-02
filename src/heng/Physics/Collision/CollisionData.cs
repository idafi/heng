namespace heng.Physics
{
	internal struct CollisionData
	{
		public readonly IPhysicsObject Object;
		public readonly Vector2 MTV;

		public CollisionData(IPhysicsObject obj, Vector2 mtv)
		{
			Assert.Ref(obj);

			Object = obj;
			MTV = mtv;
		}
	};
}