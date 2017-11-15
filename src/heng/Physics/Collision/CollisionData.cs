namespace heng.Physics
{
	/// <summary>
	/// Describes a collision between two <see cref="IPhysicsBody"/> instances.
	/// <para>This is sent to the <see cref="IPhysicsBody"/>'s <see cref="IPhysicsBody.CollisionPass"/> method.
	/// The <see cref="IPhysicsBody"/> is then responsible for enacting appropriate response behavior.</para>
	/// </summary>
	public struct CollisionData
	{
		/// <summary>
		/// The <see cref="IPhysicsBody"/> being collided with.
		/// </summary>
		public readonly IPhysicsBody Other;

		/// <summary>
		/// The minimum translation vector needed to de-intersect from the other <see cref="IPhysicsBody"/>.
		/// </summary>
		public readonly Vector2 MTV;

		/// <summary>
		/// The collision normal, perpendicular to the impacted surface.
		/// <para>(This is just a normalized MTV vector.)</para>
		/// </summary>
		public readonly Vector2 Normal;

		internal CollisionData(IPhysicsBody other, Vector2 mtv)
		{
			Assert.Ref(other);

			Other = other;
			MTV = mtv;
			Normal = mtv.Normalize();
		}
	};
}