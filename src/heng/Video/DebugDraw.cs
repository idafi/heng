using System.Collections.Generic;

namespace heng.Video
{
	/// <summary>
	/// Provides methods to quickly draw primitives to open <see cref="Window"/>s.
	/// <para>When a <see cref="VideoState"/> is constructed, debug-draw primitives will be drawn on every
	/// window, after everything else has drawn.</para>
	/// This is only a debugging helper, and is disabled if the DEBUG macro is not defined.
	/// </summary>
	public static class DebugDraw
	{
#if DEBUG
		static Queue<IDrawable> drawables;

		static DebugDraw()
		{
			drawables = new Queue<IDrawable>();
		}

		/// <summary>
		/// Draws a world-space pixel-width line.
		/// </summary>
		/// <param name="start">The line's start point.</param>
		/// <param name="end">The line's end point.</param>
		/// <param name="color">The line's color.</param>
		public static void DrawLine(WorldPoint start, WorldPoint end, Color color)
		{
			drawables.Enqueue(new LineDrawable(start, end, color));
		}

		/// <summary>
		/// Draws a pixel-width line, using a world-space origin and a pixel-space <see cref="Vector2"/>.
		/// </summary>
		/// <param name="position">The line's origin point.</param>
		/// <param name="vector">The vector describing the line's length and direction.</param>
		/// <param name="color">The line's color.</param>
		public static void DrawVector(WorldPoint position, Vector2 vector, Color color)
		{
			drawables.Enqueue(new VectorDrawable(vector, position, color));
		}

		internal static IEnumerable<IDrawable> PumpDrawables()
		{
			foreach(IDrawable drw in drawables)
			{ yield return drw; }

			drawables.Clear();
		}
#else
		public static void DrawLine(WorldPoint start, WorldPoint end, Color color) { }
		public static void DrawVector(WorldPoint position, Vector2 vector, Color color) { }
		internal static IEnumerable<IDrawable> PumpDrawables() { return new IDrawable[0]; }
#endif
	};
}