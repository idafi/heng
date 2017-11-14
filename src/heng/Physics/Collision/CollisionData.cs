namespace heng.Physics
{
	internal struct CollisionData
	{
		public readonly IPhysicsObject Other;

		public readonly Vector2 MTV;
		public readonly Vector2 Normal;
		public readonly Vector2 VelocityChange;

		public CollisionData(IPhysicsObject other, Vector2 mtv, Vector2 normal, Vector2 change)
		{
			Assert.Ref(other);

			Other = other;
			MTV = mtv;
			Normal = normal;
			VelocityChange = change;
		}
	};
}