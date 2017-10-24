using System.Runtime.InteropServices;

namespace heng
{
	internal static class Core
	{
		const string coreLib = "hcore";

		[DllImport(coreLib, EntryPoint = "Core_Test")]
		public static extern void Test();
	};
}