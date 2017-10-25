using System.Diagnostics;

namespace heng
{
	/// <summary>
	/// Provides assertion methods, which halt the program upon encountering logic errors.
	/// </summary>
	public static class Assert
	{
		/// <summary>
		/// Asserts that given condition is true, printing a message if not.
		/// </summary>
		/// <param name="condition">The condition which should be true.</param>
		/// <param name="msg">The message to print if the assertion fails.</param>
		public static void Cond(bool condition, string msg)
		{
			Debug.Assert(condition, msg);
		}
		
		/// <summary>
		/// Asserts that the given object reference is not null.
		/// </summary>
		/// <param name="reference">The reference that shouldn't be null.</param>
		public static void Ref(object reference)
		{
			Debug.Assert(reference != null, "reference is null");
		}
		
		/// <summary>
		/// Asserts that the given index is valid (greater than -1, less than the given range.)
		/// </summary>
		/// <param name="i">The index value to check.</param>
		/// <param name="max">The valid range - the index should be less than this.</param>
		public static void Index(int i, int max)
		{
			Debug.Assert(i > -1 && i < max, $"invalid index: expected within 0 to {max - 1}, got {i}");
		}
		
		/// <summary>
		/// Asserts that the given count is valid (greater than -1, less than or equal to the given maximum.)
		/// </summary>
		/// <param name="ct">The count value to check.</param>
		/// <param name="max">The valid range - the count should be less than or equal to this.</param>
		public static void Count(int ct, int max)
		{
			Debug.Assert(ct > -1 && ct <= max, $"invalid count: expected within 0 to {max}, got {ct}");
		}
		
		/// <summary>
		/// Asserts that the given value is greater than zero.
		/// </summary>
		/// <param name="s">The value to check.</param>
		public static void Sign(sbyte s) => Sign((long)(s));

		/// <summary>
		/// Asserts that the given value is greater than zero.
		/// </summary>
		/// <param name="s">The value to check.</param>
		public static void Sign(short s) => Sign((long)(s));

		/// <summary>
		/// Asserts that the given value is greater than zero.
		/// </summary>
		/// <param name="i">The value to check.</param>
		public static void Sign(int i) => Sign((long)(i));

		/// <summary>
		/// Asserts that the given value is greater than zero.
		/// </summary>
		/// <param name="f">The value to check.</param>
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