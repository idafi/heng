namespace heng
{
	/// <summary>
	/// A position in 2D space using virtualized <see cref="WorldCoordinate"/>s.
	/// <para>A WorldPoint is represented as an integer-based "sector" position, and a 0-1 fractional
	/// "subposition" within that sector.</para>
	/// A sector's actual size is determined by the <see cref="WorldCoordinate.PixelsPerSector"/> constant.
	/// <para>WorldPoints can interact with pixel-space  <see cref="Vector2"/> values using
	/// <see cref="PixelDistance(WorldPoint)"/> and <see cref="PixelTranslate(Vector2)"/></para>
	/// </summary>
	public struct WorldPoint
	{
		/// <summary>
		/// This WorldPoint's X coordinate.
		/// </summary>
		public readonly WorldCoordinate X;

		/// <summary>
		/// This WorldPoint's Y coordinate.
		/// </summary>
		public readonly WorldCoordinate Y;
		
		/// <summary>
		/// Shorctcut for a (0, 0), (0, 0) <see cref="WorldPoint"/>.
		/// </summary>
		public static WorldPoint Zero => new WorldPoint(WorldCoordinate.Zero, WorldCoordinate.Zero);

		/// <summary>
		/// This WorldPoint's Sector position, as a <see cref="Vector2"/>.
		/// </summary>
		public Vector2 Sector => new Vector2(X.Sector, Y.Sector);
		
		/// <summary>
		/// This WorldPoint's Subposition, as a <see cref="Vector2"/>.
		/// </summary>
		public Vector2 Subposition => new Vector2(X.Subposition, Y.Subposition);
		
		/// <summary>
		/// Constructs a new <see cref="WorldPoint"/> using the given <see cref="WorldCoordinate"/>s.
		/// </summary>
		/// <param name="x">The new <see cref="WorldPoint"/>'s X coordinate.</param>
		/// <param name="y">The new <see cref="WorldPoint"/>'s Y coordinate.</param>
		public WorldPoint(WorldCoordinate x, WorldCoordinate y)
		{
			X = x;
			Y = y;
		}
		
		/// <summary>
		/// Construct a new <see cref="WorldPoint"/> at the given sector and subposition.
		/// </summary>
		/// <param name="sectorX">The new <see cref="WorldPoint"/>'s X sector coordinate.</param>
		/// <param name="sectorY">The new <see cref="WorldPoint"/>'s Y sector coordinate.</param>
		/// <param name="subposition">The new <see cref="WorldPoint"/>'s subposition coordinates.</param>
		public WorldPoint(int sectorX, int sectorY, Vector2 subposition)
		{
			X = new WorldCoordinate(sectorX, subposition.X);
			Y = new WorldCoordinate(sectorY, subposition.Y);
		}
		
		/// <summary>
		/// Construct a new <see cref="WorldPoint"/> at the given sector and subposition.
		/// </summary>
		/// <param name="sector">The new <see cref="WorldPoint"/>'s sector coordinates.</param>
		/// <param name="subposition">The new <see cref="WorldPoint"/>'s subposition coordinates.</param>
		public WorldPoint(Vector2 sector, Vector2 subposition)
		{
			X = new WorldCoordinate(HMath.FloorToInt(sector.X), subposition.X);
			Y = new WorldCoordinate(HMath.FloorToInt(sector.Y), subposition.Y);
		}

		/// <inheritdoc />
		public static WorldPoint operator +(WorldPoint a, WorldPoint b)
			=> new WorldPoint(a.X + b.X, a.Y + b.Y);

		/// <inheritdoc />
		public static WorldPoint operator -(WorldPoint a, WorldPoint b)
			=> new WorldPoint(a.X - b.X, a.Y - b.Y);
		
		/// <inheritdoc />
		public static bool operator ==(WorldPoint a, WorldPoint b)
			=> (a.X == b.X) && (a.Y == b.Y);
		
		/// <inheritdoc />
		public static bool operator !=(WorldPoint a, WorldPoint b)
			=> (a.X != b.X) || (a.Y != b.Y);

		/// <summary>
		/// Linearly interpolates between two <see cref="WorldPoint"/>s.
		/// </summary>
		/// <param name="a">The interpolation start point.</param>
		/// <param name="b">The interpolation end point.</param>
		/// <param name="t">A 0-1 fraction, indicating which value between the two points to return.</param>
		/// <returns>A <see cref="WorldPoint"/> value between the start and end point.</returns>
		public static WorldPoint Lerp(WorldPoint a, WorldPoint b, float t)
		{
			WorldCoordinate x = WorldCoordinate.Lerp(a.X, b.X, t);
			WorldCoordinate y = WorldCoordinate.Lerp(a.Y, b.Y, t);
			
			return new WorldPoint(x, y);
		}

		/// <inheritdoc />
		public override bool Equals(object obj) => base.Equals(obj);

		/// <inheritdoc />
		public override int GetHashCode() => base.GetHashCode();

		/// <inheritdoc />
		public override string ToString() => $"({X.Sector}, {Y.Sector}), ({X.Subposition}, {Y.Subposition})";

		/// <summary>
		/// Clamps this <see cref="WorldPoint"/> between two other <see cref="WorldPoint"/> values.
		/// </summary>
		/// <param name="lower">The lower bound to clamp between.</param>
		/// <param name="upper">The upper bound to clamp between.</param>
		/// <returns>A new <see cref="WorldPoint"/>, clamped within the given bounds.</returns>
		public WorldPoint Clamp(WorldPoint lower, WorldPoint upper)
		{
			WorldCoordinate x = this.X.Clamp(lower.X, upper.X);
			WorldCoordinate y = this.Y.Clamp(lower.Y, upper.Y);
			
			return new WorldPoint(x, y);
		}
		
		/// <summary>
		/// Translates this <see cref="WorldPoint"/> by the given pixel-space vector..
		/// </summary>
		/// <param name="dist">The pixel-space vector by which to translate this <see cref="WorldPoint"/>.</param>
		/// <returns>A new <see cref="WorldPoint"/>, translated by the given pixel-space vector.</returns>
		public WorldPoint PixelTranslate(Vector2 dist)
		{
			WorldCoordinate x = this.X.PixelTranslate(dist.X);
			WorldCoordinate y = this.Y.PixelTranslate(dist.Y);
			
			return new WorldPoint(x, y);
		}
		
		/// <summary>
		/// Returns a pixel-space vector between this <see cref="WorldPoint"/> and another.
		/// </summary>
		/// <param name="other">The other <see cref="WorldPoint"/>.</param>
		/// <returns>The pixel-space vector between the two <see cref="WorldPoint"/>s.</returns>
		public Vector2 PixelDistance(WorldPoint other)
		{
			float x = this.X.PixelDistance(other.X);
			float y = this.Y.PixelDistance(other.Y);
			
			return new Vector2(x, y);
		}
		
		/// <summary>
		/// Finds the least common sector between this <see cref="WorldPoint"/> and another.
		/// </summary>
		/// <param name="other">The other <see cref="WorldPoint"/>.</param>
		/// <returns>A new <see cref="WorldPoint"/>, located at the origin of the least common sector.</returns>
		public WorldPoint LeastCommonSector(WorldPoint other)
		{
			int xSec = HMath.Min(this.X.Sector, other.X.Sector);
			int ySec = HMath.Min(this.Y.Sector, other.Y.Sector);

			return new WorldPoint(new WorldCoordinate(xSec, 0), new WorldCoordinate(ySec, 0));
		}
	};
}