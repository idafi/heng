using System.Diagnostics;

namespace heng
{
	public static class Assert
	{
		public static void Cond(bool condition, string msg)
		{
			Debug.Assert(condition, msg);
		}
		
		public static void Ref(object reference)
		{
			Debug.Assert(reference != null, "reference is null");
		}
		
		public static void Index(int i, int max)
		{
			Debug.Assert(i > -1 && i < max, $"invalid index: expected within 0 to {max - 1}, got {i}");
		}
		
		public static void Count(int ct, int max)
		{
			Debug.Assert(ct > -1 && ct <= max, $"invalid count: expected within 0 to {max}, got {ct}");
		}
		
		public static void Sign(sbyte s) => Sign((long)(s));
		public static void Sign(short s) => Sign((long)(s));
		public static void Sign(int i) => Sign((long)(i));
		public static void Sign(float f) => Sign((double)(f));
		
		static void Sign(long l)
		{
			Debug.Assert(l >= 0, $"expected positive integral, got {l}");
		}
		
		static void Sign(double d)
		{
			Debug.Assert(d >= 0.0, $"expected positive floating point, got {d}");
		}
	};
}