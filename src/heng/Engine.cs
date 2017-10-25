namespace heng
{
	public static class Engine
	{
		public static bool Init(CoreConfig config) => Core.Init(config);
		public static void Quit() => Core.Quit();
	};
}