using System;

namespace heng.Video
{
	/// <summary>
	/// Represents a texture drawn to a <see cref="Window"/>.
	/// <para>The texture file string used to construct the <see cref="Sprite"/> hashed into
	/// an internal resource map, which ensures multiple <see cref="Sprite"/>s using the same
	/// texture file don't lead to multiple redundant resource loads.</para>
	/// </summary>
	public class Sprite : IDisposable
	{
		/// <summary>
		/// The pixel-space position at which to draw the texture.
		/// </summary>
		public readonly ScreenPoint Position;

		/// <summary>
		/// The texture will be rotated by this many degrees when it is drawn.
		/// </summary>
		public readonly float Rotation;

		readonly int textureID;

		/// <summary>
		/// Constructs a new <see cref="Sprite"/>.
		/// </summary>
		/// <param name="textureFile">The file from which this <see cref="Sprite"/>'s texture will be loaded.</param>
		/// <param name="position">The position at which to draw the texture.</param>
		/// <param name="rotation">The texture will be rotated by this many degrees when it is drawn.</param>
		public Sprite(string textureFile, ScreenPoint position, float rotation)
		{
			Position = position;
			Rotation = rotation;

			textureID = Core.Video.Textures.LoadTexture(textureFile);
		}

		/// <summary>
		/// Releases the texture resource used by this <see cref="Sprite"/>.
		/// <para>When multiple <see cref="Sprite"/>s share the same texture file, the underlying resource map
		/// will only free the texture file when all of those <see cref="Sprite"/>s are disposed.</para>
		/// </summary>
		public void Dispose()
		{
			Core.Video.Textures.FreeTexture(textureID);
		}

		/// <summary>
		/// Draws the <see cref="Sprite"/> to the given window.
		/// </summary>
		/// <param name="window">The window to which the <see cref="Sprite"/> should be drawn.</param>
		public void Draw(Window window)
		{
			window.DrawTexture(textureID, Position, Rotation);
		}
	};
}