namespace heng.Physics
{
	public class StaticBody : IPhysicsObject
	{
		public readonly Vector2 Position;
		public readonly ICollider Collider;

		Vector2 IPhysicsObject.Position => Position;
		ICollider IPhysicsObject.Collider => Collider;
	
		public StaticBody(Vector2 position, ICollider collider)
		{
			Position = position;
			Collider = collider;
		}
	};
}