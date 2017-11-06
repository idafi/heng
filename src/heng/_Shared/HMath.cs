using System;

namespace heng
{
	/// <summary>
	/// Helper methods for math functions.
	/// </summary>
	public static class HMath
	{
		/// <summary>
		/// Returns the largest value within the given set.
		/// </summary>
		/// <param name="values">The set of values to choose from.</param>
		/// <returns>The largest value in the set.</returns>
		public static float Max(params float[] values)
		{
			Assert.Ref(values);
			Assert.Cond(values.Length > 0, "array has no values");

			float max = values[0];
			for(int i = 1; i < values.Length; i++)
			{
				max = Math.Max(max, values[i]);
			}

			return max;
		}

		/// <summary>
		/// Returns the largest value within the given set.
		/// </summary>
		/// <param name="values">The set of values to choose from.</param>
		/// <returns>The largest value in the set.</returns>
		public static int Max(params int[] values)
		{
			Assert.Ref(values);
			Assert.Cond(values.Length > 0, "array has no values");

			int max = values[0];
			for(int i = 1; i < values.Length; i++)
			{
				max = Math.Max(max, values[i]);
			}

			return max;
		}

		/// <summary>
		/// Returns the smallest value within the given set.
		/// </summary>
		/// <param name="values">The set of values to choose from.</param>
		/// <returns>The smallest value in the set.</returns>
		public static float Min(params float[] values)
		{
			Assert.Ref(values);
			Assert.Cond(values.Length > 0, "array has no values");

			float min = values[0];
			for(int i = 1; i < values.Length; i++)
			{
				min = Math.Min(min, values[i]);
			}

			return min;
		}

		/// <summary>
		/// Returns the smallest value within the given set.
		/// </summary>
		/// <param name="values">The set of values to choose from.</param>
		/// <returns>The smallest value in the set.</returns>
		public static int Min(params int[] values)
		{
			Assert.Ref(values);
			Assert.Cond(values.Length > 0, "array has no values");

			int min = values[0];
			for(int i = 1; i < values.Length; i++)
			{
				min = Math.Min(min, values[i]);
			}

			return min;
		}

		/// <summary>
		/// Clamps a value between the given lower and upper bounds.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="lower">The lower bound to clamp between.</param>
		/// <param name="upper">The upper bound to clamp between.</param>
		/// <returns>The clamped value.</returns>
		public static float Clamp(float value, float lower, float upper)
		{
			return Min(Max(value, lower), upper);
		}

		/// <summary>
		/// Clamps a value between the given lower and upper bounds.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="lower">The lower bound to clamp between.</param>
		/// <param name="upper">The upper bound to clamp between.</param>
		/// <returns>The clamped value.</returns>
		public static int Clamp(int value, int lower, int upper)
		{
			return Min(Max(value, lower), upper);
		}

		/// <summary>
		/// Single-precision float wrapper around <see cref="Math.Floor(double)"/>.
		/// </summary>
		/// <param name="f">The float to floor.</param>
		/// <returns>The floored float.</returns>
		public static float Floor(float f)
		{
			return Convert.ToSingle(Math.Floor(f));
		}

		/// <summary>
		/// Single-precision float wrapper around <see cref="Math.Floor(double)"/>, that returns as int.
		/// </summary>
		/// <param name="f">The float to floor.</param>
		/// <returns>The floored float, as an int.</returns>
		public static int FloorToInt(float f)
		{
			return Convert.ToInt32(Math.Floor(f));
		}

		/// <summary>
		/// Single-precision float wrapper around <see cref="Math.Ceiling(double)"/>.
		/// </summary>
		/// <param name="f">The float to ceiling.</param>
		/// <returns>The ceilinged float.</returns>
		public static float Ceil(float f)
		{
			return Convert.ToSingle(Math.Ceiling(f));
		}

		/// <summary>
		/// Single-precision float wrapper around <see cref="Math.Ceiling(double)"/>, that returns as int.
		/// </summary>
		/// <param name="f">The float to ceiling.</param>
		/// <returns>The ceilinged float, as an int.</returns>
		public static int CeilToInt(float f)
		{
			return Convert.ToInt32(Math.Ceiling(f));
		}

		/// <summary>
		/// Single-precision float wrapper around <see cref="Math.Sqrt(double)"/>.
		/// </summary>
		/// <param name="f">The float to sqrt.</param>
		/// <returns>The squre-rooted float.</returns>
		public static float Sqrtf(float f)
		{
			return Convert.ToSingle(Math.Sqrt(f));
		}

		// TODO: arbitrary min
		/// <summary>
		/// Wraps the given value between 0 and the given max bound.
		/// </summary>
		/// <param name="value">The value to wrap.</param>
		/// <param name="max">The max bound, after which the value should start wrapping.</param>
		/// <returns>The wrapped value.</returns>
		public static float Wrap(float value, float max)
		{
			value %= max;
			if(value < 0) { value += max; }

			return value;
		}

		/// <summary>
		/// Wraps the given value between 0 and the given max bound.
		/// </summary>
		/// <param name="value">The value to wrap.</param>
		/// <param name="max">The max bound, after which the value should start wrapping.</param>
		/// <returns>The wrapped value.</returns>
		public static int Wrap(int value, int max)
		{
			value %= max;
			if(value < 0) { value += max; }

			return value;
		}

		/// <summary>
		/// Single-precision float wrapper around <see cref="Math.Pow(double, double)"/>.
		/// </summary>
		/// <param name="x">The float to raise.</param>
		/// <param name="y">The exponent.</param>
		/// <returns>The resulting float value.</returns>
		public static float Pow(float x, float y)
		{
			return Convert.ToSingle(Math.Pow(x, y));
		}

		/// <summary>
		/// int wrapper around <see cref="Math.Pow(double, double)"/>.
		/// </summary>
		/// <param name="x">The int to raise.</param>
		/// <param name="y">The exponent.</param>
		/// <returns>The resulting int value.</returns>
		public static int Pow(int x, int y)
		{
			return Convert.ToInt32(Math.Pow(x, y));
		}

		// "why?" i'm a weirdo who feels better writing value once, rather than twice (value*value)
		/// <summary>
		/// Squares the given value.
		/// </summary>
		/// <param name="value">The value to square.</param>
		/// <returns>The squared value.</returns>
		public static float Square(float value)
		{
			return value * value;
		}

		/// <summary>
		/// Squares the given value.
		/// </summary>
		/// <param name="value">The value to square.</param>
		/// <returns>The squared value.</returns>
		public static int Square(int value)
		{
			return value * value;
		}

		/// <summary>
		/// Linearly interpolates betweent two values.
		/// </summary>
		/// <param name="a">The interpolation start point.</param>
		/// <param name="b">The interpolation end point.</param>
		/// <param name="t">A 0-1 fraction, indicating which value between the two points to return.</param>
		/// <returns>A value between the start and end point.</returns>
		public static float Lerp(float a, float b, float t)
		{
			float diff = b - a;
			return a + (diff * t);
		}

		/// <summary>
		/// Linearly interpolates betweent two values.
		/// </summary>
		/// <param name="a">The interpolation start point.</param>
		/// <param name="b">The interpolation end point.</param>
		/// <param name="t">A 0-1 fraction, indicating which value between the two points to return.</param>
		/// <returns>A value between the start and end point.</returns>
		public static int Lerp(int a, int b, float t)
		{
			int diff = b - a;
			float add = diff * t;

			return a + Convert.ToInt32(add);
		}
	};
}