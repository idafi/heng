namespace heng.Physics
{
	public interface IPhysicsObject
	{
		Vector2 Position { get; }
		ICollider Collider  { get; }
	};
}