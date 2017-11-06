using System;

namespace heng.Video
{
	/// <summary>
	/// Represents a texture resource, loaded from file.
	/// <para>The underlying resource map will ensure texture resources using the same file will not
	/// incur duplicate file loads, if a <see cref="Texture"/> using the same file is already loaded.</para>
	/// Be sure to <see cref="Dispose"/> of the <see cref="Texture"/> when it's no longer needed.
	/// </summary>
	public class Texture : IDisposable
	{
		readonly int textureID;
		bool isDisposed;

		/// <summary>
		/// Creates a new <see cref="Texture"/> from the texture file at the given path.
		/// <para>Textures are meant to be constructed into <see cref="Sprite"/>s, which are then
		/// built (along with other <see cref="IDrawable"/> objects> into the <see cref="VideoState"/>.</para>
		/// </summary>
		/// <param name="filePath">The file from which to create the new <see cref="Texture"/>.</param>
		public Texture(string filePath)
		{
			textureID = Core.Video.Textures.LoadTexture(filePath);
			if(textureID < 0)
			{ isDisposed = true; }
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if(!isDisposed)
			{
				Core.Video.Textures.FreeTexture(textureID);
				isDisposed = true;
			}
			else
			{ Log.Warning("tried to Dispose of an already-disposed Texture"); }
		}

		internal void Draw(int windowID, ScreenPoint position, float rotation)
		{
			if(textureID > -1)
			{ Core.Video.Queue.DrawTexture(windowID, textureID, position, rotation); }
			else
			{ Log.Warning($"couldn't draw texture to window {windowID}: texture was disposed, or erroneously constructed"); }
		}
	};
}