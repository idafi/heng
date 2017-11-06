namespace heng.Video
{
	/// <summary>
	/// A world-space instance of a <see cref="Video.Texture"/>, drawable to a <see cref="Window"/>.
	/// </summary>
	public class Sprite : IDrawable
	{
		/// <summary>
		/// The <see cref="Video.Texture"/> to draw.
		/// </summary>
		public readonly Texture Texture;

		/// <summary>
		/// The world-space position at which to draw this <see cref="Sprite"/>.
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// The number of degrees to rotate the <see cref="Texture"/> when drawn.
		/// </summary>
		public readonly float Rotation;
		
		/// <summary>
		/// Constructs a new <see cref="Sprite"/>.
		/// </summary>
		/// <param name="texture">The new <see cref="Sprite"/>'s <see cref="Video.Texture"/>.</param>
		/// <param name="position">The new <see cref="Sprite"/>'s world-space position.</param>
		/// <param name="rotation">The new <see cref="Sprite"/>'s rotation, in degrees.</param>
		public Sprite(Texture texture, WorldPoint position, float rotation)
		{
			Texture = texture;
			Position = position;
			Rotation = rotation;
			
			if(texture == null)
			{ Log.Warning("constructed Sprite with null texture"); }
		}
		
		/// <summary>
		/// Repositions this <see cref="Sprite"/> to the given world-space position.
		/// </summary>
		/// <param name="position">The new world-space position for the <see cref="Sprite"/>.</param>
		/// <returns>A new repositioned <see cref="Sprite"/>.</returns>
		public Sprite Reposition(WorldPoint position)
		{
			return new Sprite(Texture, position, Rotation);
		}
		
		/// <inheritdoc />
		public void Draw(Window window)
		{
			Assert.Ref(window);

			ScreenPoint pixelPos = (ScreenPoint)(Position.PixelPosition);
			window.DrawTexture(Texture, pixelPos, Rotation);
		}
	};
}