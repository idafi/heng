namespace heng
{
	/// <summary>
	/// A virtualized coordinate position.
	/// <para>A WorldCoordinate is represented as an integer-based "sector," and a 0-1 fractional "subposition"
	/// within that sector.</para>
	/// A sector's actual size is determined by the <see cref="PixelsPerSector"/> constant.
	/// </summary>
	public struct WorldCoordinate
	{
		/// <summary>
		/// How many pixels comprise a single sector.
		/// </summary>
		public const int PixelsPerSector = 200;

		/// <summary>
		/// This <see cref="WorldCoordinate"/>'s sector position.
		/// </summary>
		public readonly int Sector;

		/// <summary>
		/// The fractional subposition inside this <see cref="WorldCoordinate"/>'s sector.
		/// </summary>
		public readonly float Subposition;

		/// <summary>
		/// Shortcut for new WorldCoordinate(0, 0).
		/// </summary>
		public static WorldCoordinate Zero => new WorldCoordinate(0, 0);

		/// <summary>
		/// Constructs a new WorldCoordinate at the given position.
		/// <para>If clampSubposition is false, subposition values greater than 1 will be added to the
		/// new <see cref="WorldCoordinate"/>'s sector position.</para>
		/// For instance, setting the sector to 1 and subposition 1.5 would place the new <see cref="WorldCoordinate"/>
		/// at sector 2, subposition 0.5.
		/// </summary>
		/// <param name="sector">The new <see cref="WorldCoordinate"/>'s sector position.</param>
		/// <param name="subposition">The new <see cref="WorldCoordinate"/>'s subposition within its sector.</param>
		/// <param name="clampSubposition">Whether to clamp subposition between 0 and 1, or add excess
		/// position to the sector value.</param>
		public WorldCoordinate(int sector, float subposition, bool clampSubposition = true)
		{
			if(clampSubposition)
			{
				Sector = sector;
				Subposition = HMath.Clamp(subposition, 0, 1 - float.Epsilon);
			}
			else
			{	
				int diff = HMath.FloorToInt(subposition);

				Sector = sector + diff;
				Subposition = subposition - diff;
			}
		}

		/// <inheritdoc />
		public static WorldCoordinate operator +(WorldCoordinate a, WorldCoordinate b)
		{
			int sec = a.Sector + b.Sector;
			float sub = a.Subposition + b.Subposition;
			
			return new WorldCoordinate(sec, sub, false);
		}
		
		/// <inheritdoc />
		public static WorldCoordinate operator -(WorldCoordinate a, WorldCoordinate b)
		{
			int sec = a.Sector - b.Sector;
			float sub = a.Subposition - b.Subposition;
			
			return new WorldCoordinate(sec, sub, false);
		}

		/// <inheritdoc />
		public static bool operator ==(WorldCoordinate a, WorldCoordinate b)
		{
			return (a.Sector == b.Sector) && (a.Subposition == b.Subposition);
		}
		
		/// <inheritdoc />
		public static bool operator !=(WorldCoordinate a, WorldCoordinate b)
		{
			return (a.Sector != b.Sector) || (a.Subposition != b.Subposition);
		}
		
		/// <inheritdoc />
		public static bool operator >(WorldCoordinate a, WorldCoordinate b)
		{
			return (a.Sector == b.Sector) ? (a.Subposition > b.Subposition) : (a.Sector > b.Sector);
		}
		
		/// <inheritdoc />
		public static bool operator <(WorldCoordinate a, WorldCoordinate b)
		{
			return (a.Sector == b.Sector) ? (a.Subposition < b.Subposition) : (a.Sector < b.Sector);
		}

		/// <summary>
		/// Linearly interpolates between two <see cref="WorldCoordinate"/>s.
		/// </summary>
		/// <param name="a">The interpolation start point.</param>
		/// <param name="b">The interpolation end point.</param>
		/// <param name="t">A 0-1 fraction, indicating which value between the two points to return.</param>
		/// <returns>A <see cref="WorldCoordinate"/> value between the start and end point.</returns>
		public static WorldCoordinate Lerp(WorldCoordinate a, WorldCoordinate b, float t)
		{
			WorldCoordinate diff = b - a;
			
			float secAdd = diff.Sector * t;
			float subAdd = diff.Subposition * t;
			float secRem = secAdd % 1;
			
			int sec = a.Sector + HMath.FloorToInt(secAdd);
			float sub = a.Subposition + subAdd + secRem;
			return new WorldCoordinate(sec, sub, false);
		}

		/// <inheritdoc />
		public override bool Equals(object obj) => base.Equals(obj);

		/// <inheritdoc />
		public override int GetHashCode() => base.GetHashCode();

		/// <inheritdoc />
		public override string ToString() => $"{Sector}{Subposition:.#########}";

		/// <summary>
		/// Clamps this <see cref="WorldCoordinate"/> between two other <see cref="WorldCoordinate"/> values.
		/// </summary>
		/// <param name="lower">The lower bound to clamp between.</param>
		/// <param name="upper">The upper bound to clamp between.</param>
		/// <returns>A new <see cref="WorldCoordinate"/>, clamped within the given bounds.</returns>
		public WorldCoordinate Clamp(WorldCoordinate lower, WorldCoordinate upper)
		{
			return (this < lower) ? lower : (this > upper) ? upper : this;
		}

		/// <summary>
		/// Translates this <see cref="WorldCoordinate"/> by the given number of pixels.
		/// </summary>
		/// <param name="dist">The amount of pixels by which to translate this <see cref="WorldCoordinate"/>.</param>
		/// <returns>A new <see cref="WorldCoordinate"/>, translated by the given pixel distance.</returns>
		public WorldCoordinate PixelTranslate(float dist)
		{
			dist /= PixelsPerSector;
			return new WorldCoordinate(Sector, Subposition + dist, false);
		}
		
		/// <summary>
		/// Returns the distance, in pixels, between this <see cref="WorldCoordinate"/> and another.
		/// </summary>
		/// <param name="other">The other <see cref="WorldCoordinate"/>.</param>
		/// <returns>The distance, in pixels, between the two <see cref="WorldCoordinate"/>s.</returns>
		public float PixelDistance(WorldCoordinate other)
		{
			int secDiff = this.Sector - other.Sector;
			float subDiff = this.Subposition - other.Subposition;
			float net = (secDiff + subDiff) * PixelsPerSector;
			
			return net;
		}
	};
}