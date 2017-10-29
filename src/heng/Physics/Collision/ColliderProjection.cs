using System;

namespace heng.Physics
{
	public struct ColliderProjection
	{
		public readonly float Min, Max;

		public static float GetOverlap(ColliderProjection a, ColliderProjection b)
		{
			if((a.Min <= b.Max) && (b.Min <= a.Max))
			{ return Math.Min(a.Max, b.Max) - Math.Max(a.Min, b.Min); }

			return 0;
		}

		public ColliderProjection(float min, float max)
		{
			Min = min;
			Max = max;
		}
	};
}