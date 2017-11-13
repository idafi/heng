using static System.Math;

namespace heng
{
	/// <summary>
	/// Represents a 2-dimensional vector.
	/// </summary>
	public struct Vector2
	{
		/// <summary>
		/// The vector's X component.
		/// </summary>
		public readonly float X;

		/// <summary>
		/// The vector's Y component.
		/// </summary>
		public readonly float Y;

		/// <summary>
		/// Shortcut for new Vector2(0, 0).
		/// </summary>
		public static Vector2 Zero => new Vector2(0, 0);

		/// <summary>
		/// Shortcut for new Vector2(1, 0).
		/// </summary>
		public static Vector2 Right => new Vector2(1, 0);

		/// <summary>
		/// Shortcut for new Vector2(0, 1).
		/// </summary>
		public static Vector2 Up => new Vector2(0, 1);

		/// <summary>
		/// Shortcut for new Vector2(-1, 0).
		/// </summary>
		public static Vector2 Left => new Vector2(-1, 0);

		/// <summary>
		/// Shortcut for new Vector2(0, -1).
		/// </summary>
		public static Vector2 Down => new Vector2(0, -1);

		/// <summary>
		/// The length of this vector, squared.
		/// <para>If this is enough to get the job done, it can save a little bit of time over
		/// calculating the full magnitude, skipping a potentially-expensive square root operation.</para>
		/// </summary>
		public float SqrMagnitude => Dot(this);

		/// <summary>
		/// The length of this vector.
		/// </summary>
		public float Magnitude => (float)(Sqrt(SqrMagnitude));

		/// <summary>
		/// A perpendicular vector of equal magnitude, facing left of this vector's direction.
		/// </summary>
		public Vector2 LeftNormal => new Vector2(Y, -X);

		/// <summary>
		/// A perpendicular vector of equal magnitude, facing right of this vector's direction.
		/// </summary>
		public Vector2 RightNormal => new Vector2(-Y, X);

		/// <summary>
		/// Constructs a new <see cref="Vector2"/> using the given components.
		/// </summary>
		/// <param name="x">The new vector's X component.</param>
		/// <param name="y">The new vector's Y component.</param>
		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Calculates and constructs a new <see cref="Vector2"/> from the given angle, in degrees.
		/// </summary>
		/// <param name="degrees">The angle, in degrees, from which the new vector will be calculated.</param>
		public Vector2(float degrees)
		{
			float rad = (float)(degrees / (180.0f / PI));
			X = (float)(Cos(rad));
			Y = (float)(Sin(rad));
		}

		/// <summary>
		/// Adds two <see cref="Vector2"/>s, returning a new <see cref="Vector2"/> representing the
		/// sum of their components.
		/// </summary>
		/// <param name="a">The first vector to add.</param>
		/// <param name="b">The second vector to add.</param>
		/// <returns>A new <see cref="Vector2"/>, representing the sum of the operands' components.</returns>
		public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);

		/// <summary>
		/// Calculates a new <see cref="Vector2"/> between the two given <see cref="Vector2"/>s.
		/// <para>The new vector will be pointing toward the first operand.</para>
		/// </summary>
		/// <param name="a">The vector from which to subtract.</param>
		/// <param name="b">The vector to subtract.</param>
		/// <returns>A new <see cref="Vector2"/> between the two operands, pointing toward the first.</returns>
		public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);

		/// <summary>
		/// Scales a <see cref="Vector2"/> by the given scalar.
		/// </summary>
		/// <param name="v">The vector to scale.</param>
		/// <param name="s">The scalar.</param>
		/// <returns>A new <see cref="Vector2"/>, scaled by the scalar.</returns>
		public static Vector2 operator *(Vector2 v, float s) => new Vector2(v.X * s, v.Y * s);

		/// <summary>
		/// Inverts a <see cref="Vector2"/>'s components, reflecting it over its normal.
		/// </summary>
		/// <param name="v">The vector to invert.</param>
		/// <returns>The inverted vector.</returns>
		public static Vector2 operator -(Vector2 v) => (v * -1);

		/// <summary>
		/// Tests if two <see cref="Vector2"/>s' components are equal.
		/// <para>This a float-equality operation, so rely on it sparingly.</para>
		/// </summary>
		/// <param name="a">The first vector to test.</param>
		/// <param name="b">The second vector to test.</param>
		/// <returns>True if both vectors' components are equal; false if not.</returns>
		public static bool operator ==(Vector2 a, Vector2 b) => (a.X == b.X && a.Y == b.Y);

		/// <summary>
		/// Tests if two <see cref="Vector2"/>s' components are not equal.
		/// <para>This is a float-equality operation, so rely on it sparingly.</para>
		/// </summary>
		/// <param name="a">The first vector to test.</param>
		/// <param name="b">The second vector to test.</param>
		/// <returns>True if the vectors' components are not equal; false if they are.</returns>
		public static bool operator !=(Vector2 a, Vector2 b) => (a.X != b.X || a.Y != b.Y);

		/// <summary>
		/// Linearly interpolates betweent two <see cref="Vector2"/>s.
		/// </summary>
		/// <param name="a">The interpolation start point.</param>
		/// <param name="b">The interpolation end point.</param>
		/// <param name="t">A 0-1 fraction, indicating which value between the two points to return.</param>
		/// <returns>A value between the start and end point.</returns>
		public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => a + ((b - a) * t);

		/// <inheritdoc />
		public override bool Equals(object obj) => base.Equals(obj);

		/// <inheritdoc />
		public override int GetHashCode() => base.GetHashCode();

		/// <inheritdoc />
		public override string ToString() => $"({X}, {Y})";

		/// <summary>
		/// Calculates the dot product of this vector, against the given vector.
		/// </summary>
		/// <param name="v">The other vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public float Dot(Vector2 v) => (this.X * v.X) + (this.Y * v.Y);

		/// <summary>
		/// Calculates a new <see cref="Vector2"/> from this vector, clamping the magnitude between the given values.
		/// </summary>
		/// <param name="min">The minimum value for the new magnitude.</param>
		/// <param name="max">The maximum value for the new magnitude.</param>
		/// <returns>A new <see cref="Vector2"/>, whose magnitude is clamped between the given values.</returns>
		public Vector2 ClampMagnitude(float min, float max)
		{
			float mag = Magnitude;
			float s = 1;

			if(mag > 0)
			{
				if(mag < min)
				{ s = min / mag; }
				else if(mag > max)
				{ s = max / mag; }
			}

			return this * s;
		}

		/// <summary>
		/// Calculates a normalized representation of this vector (magnitude is 1).
		/// <para>...Unless the magnitude is 0, in which case it will stay 0.</para>
		/// </summary>
		/// <returns>The normalized vector.</returns>
		public Vector2 Normalize()
		{
			float mag = Magnitude;
			if(mag > 0)
			{
				float s = 1 / mag;
				return this * s;
			}

			return this;
		}

		/// <summary>
		/// Projects this <see cref="Vector2"/> along another.
		/// </summary>
		/// <param name="other">The vector to project against.</param>
		/// <returns>The projected vector.</returns>
		public Vector2 Project(Vector2 other)
		{
			float dot = other.Dot(this);
			float x = (dot / this.Magnitude) * this.X;
			float y = (dot / this.Magnitude) * this.Y;

			return new Vector2(x, y);
		}

		/// <summary>
		/// Calculates an angle, in degrees, from this <see cref="Vector2"/>'s direction.
		/// </summary>
		/// <returns>The angle this <see cref="Vector2"/>'s direction represents, in degrees.</returns>
		public float ToAngle()
		{
			return (float)(Atan2(Y, X) * 180.0f / PI);
		}
	};
}