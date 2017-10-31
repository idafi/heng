using System;

namespace heng.Physics
{
	/// <summary>
	/// Represents the projection of an <see cref="ICollider"/> along a 1-dimensional axis.
	/// <para>The <see cref="CollisionTester"/> uses these projections, produced by
	/// <see cref="ICollider"/>.<see cref="ICollider.Project"/>, to test for collisions.</para>
	/// </summary>
	public struct ColliderProjection
	{
		/// <summary>
		/// The minimum and maximum bounds of the projection, in pixel-space.
		/// </summary>
		public readonly float Min, Max;

		/// <summary>
		/// Calculates the overlap between two <see cref="ColliderProjection"/>s.
		/// <para>If at least one projection of two <see cref="ICollider"/>s does not overlap,
		/// the colliders do not intersect.</para>
		/// </summary>
		/// <param name="a">The first <see cref="ColliderProjection"/>.</param>
		/// <param name="b">The second <see cref="ColliderProjection"/>.</param>
		/// <returns></returns>
		public static float GetOverlap(ColliderProjection a, ColliderProjection b)
		{
			if((a.Min <= b.Max) && (b.Min <= a.Max))
			{ return Math.Min(a.Max, b.Max) - Math.Max(a.Min, b.Min); }

			return 0;
		}

		/// <summary>
		/// Constructs a new <see cref="ColliderProjection"/> using the given max and min bounds.
		/// </summary>
		/// <param name="min">The minimum bound of the new projection.</param>
		/// <param name="max">The maximum bound of the new projection.</param>
		public ColliderProjection(float min, float max)
		{
			Min = min;
			Max = max;
		}
	};
}