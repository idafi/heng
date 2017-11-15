using heng.Physics;

namespace hgame
{
	public static class PhysicsMaterialLibrary
	{
		public static readonly PhysicsMaterial Rubber;
		public static readonly PhysicsMaterial Concrete;
		public static readonly PhysicsMaterial Wood;
		public static readonly PhysicsMaterial Iron;
		public static readonly PhysicsMaterial Steel;
		public static readonly PhysicsMaterial Ice;

		public static readonly PhysicsMaterial LowFricLowRest;
		public static readonly PhysicsMaterial HighFricLowRest;
		public static readonly PhysicsMaterial LowFricHighRest;
		public static readonly PhysicsMaterial HighFricHighRest;

		static PhysicsMaterialLibrary()
		{
			Rubber = new PhysicsMaterial(1, 0.75f, 1);
			Concrete = new PhysicsMaterial(0.75f, 0.5f, 0.35f);
			Wood = new PhysicsMaterial(0.35f, 0.2f, 0.25f);
			Iron = new PhysicsMaterial(0.8f, 0.25f, 0.45f);
			Steel = new PhysicsMaterial(0.75f, 0.5f, 0.75f);
			Ice = new PhysicsMaterial(0.05f, 0.01f, 0.15f);

			LowFricLowRest = new PhysicsMaterial(0.05f, 0.01f, 0f);
			HighFricLowRest = new PhysicsMaterial(0.8f, 0.65f, 0f);
			LowFricHighRest = new PhysicsMaterial(0.05f, 0.01f, 1f);
			HighFricHighRest = new PhysicsMaterial(0.8f, 0.65f, 1f);
		}
	};
}