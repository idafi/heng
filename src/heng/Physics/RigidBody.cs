namespace heng.Physics
{
	public class RigidBody : IPhysicsObject
	{
		public readonly Vector2 Position;
		public readonly ICollider Collider;

		public readonly Vector2 TotalImpulse;

		Vector2 IPhysicsObject.Position => Position;
		ICollider IPhysicsObject.Collider => Collider;
	
		public RigidBody(Vector2 position, ICollider collider)
		{
			Position = position;
			Collider = collider;

			TotalImpulse = Vector2.Zero;
		}
	
		RigidBody(Vector2 position, ICollider collider, Vector2 totalImpulse)
		{
			Position = position;
			Collider = collider;

			TotalImpulse = totalImpulse;
		}
		
		public RigidBody Translate(Vector2 translation) => new RigidBody(Position + translation, Collider, TotalImpulse);
		public RigidBody AddImpulse(Vector2 impulse) => new RigidBody(Position, Collider, TotalImpulse + impulse);
		public RigidBody ApplyImpulses() => new RigidBody(Position + TotalImpulse, Collider, Vector2.Zero);
	};
}