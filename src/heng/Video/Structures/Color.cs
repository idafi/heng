namespace heng.Video
{
	/// <summary>
	/// Represents an RGBA color.
	/// </summary>
	public struct Color
	{
		/// <summary>
		/// The color's red component, 0 to 255.
		/// </summary>
		public readonly byte R;

		/// <summary>
		/// The color's green component, 0 to 255.
		/// </summary>
		public readonly byte G;

		/// <summary>
		/// The color's blue component, 0 to 255.
		/// </summary>
		public readonly byte B;

		/// <summary>
		/// The color's alpha component, 0 to 255.
		/// </summary>
		public readonly byte A;
	
		/// <summary>
		/// Shortcut for R: 0, G: 0, B: 0, A: 0.
		/// </summary>
		public static Color Clear => new Color(0x00, 0x00, 0x00, 0x00);

		/// <summary>
		/// Shortcut for R: 0, G: 0, B: 0, A: 255.
		/// </summary>
		public static Color Black => new Color(0x00, 0x00, 0x00, 0xFF);

		/// <summary>
		/// Shortcut for R: 255, G: 0, B: 0, A: 255.
		/// </summary>
		public static Color Red => new Color(0xFF, 0x00, 0x00, 0xFF);

		/// <summary>
		/// Shortcut for R: 0, G: 255, B: 0, A: 255.
		/// </summary>
		public static Color Green => new Color(0x00, 0xFF, 0x00, 0xFF);

		/// <summary>
		/// Shortcut for R: 0, G: 0, B: 255, A: 255.
		/// </summary>
		public static Color Blue => new Color(0x00, 0x00, 0xFF, 0xFF);

		/// <summary>
		/// Shortcut for R: 255, G: 255, B: 0, A: 255.
		/// </summary>
		public static Color Yellow => new Color(0xFF, 0xFF, 0x00, 0xFF);

		/// <summary>
		/// Shortcut for R: 255, G: 0, B: 255, A: 255.
		/// </summary>
		public static Color Magenta => new Color(0xFF, 0x00, 0xFF, 0xFF);

		/// <summary>
		/// Shortcut for R: 0, G: 255, B: 255, A: 255.
		/// </summary>
		public static Color Cyan => new Color(0x00, 0xFF, 0xFF, 0xFF);

		/// <summary>
		/// Shortcut for R: 255, G: 255, B: 255, A: 255.
		/// </summary>
		public static Color White => new Color(0xFF, 0xFF, 0xFF, 0xFF);
	
		/// <summary>
		/// Constructs a new <see cref="Color"/> using the given components.
		/// </summary>
		/// <param name="r">The new color's red component.</param>
		/// <param name="g">The new color's green component.</param>
		/// <param name="b">The new color's blue component.</param>
		/// <param name="a">The new color's alpha component.</param>
		public Color(byte r, byte g, byte b, byte a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}
	};
}