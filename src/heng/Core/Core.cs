using System.Runtime.InteropServices;

namespace heng
{
	internal static partial class Core
	{
		const string coreLib = "hcore";

		[DllImport(coreLib, EntryPoint = "Core_Init")]
		[return: MarshalAs(UnmanagedType.U1)]
		public static extern bool Init(CoreConfig config);

		[DllImport(coreLib, EntryPoint = "Core_Quit")]
		public static extern void Quit();

		[DllImport(coreLib, EntryPoint = "Core_Test")]
		public static extern void Test();
	};
}