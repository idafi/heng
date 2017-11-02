﻿using System;

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
		bool isDisposed;

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
			if(textureID < 0)
			{ isDisposed = true; }
		}

		/// <summary>
		/// Constructs a new Sprite instance, based on an old instance.
		/// <para>The Sprite's texture resource is always kept, but any other parameters set to null
		/// will carry settings over from the old instance.</para>
		/// </summary>
		/// <param name="oldSprite">The old instance upon which the new Sprite is based.</param>
		/// <param name="position">The position at which to draw the texture.</param>
		/// <param name="rotation">The texture will be rotated by this many degrees when it is drawn.</param>
		public Sprite(Sprite oldSprite, ScreenPoint? position = null, float? rotation = null)
		{
			if(oldSprite != null)
			{
				textureID = oldSprite.textureID;
				Position = position ?? oldSprite.Position;
				Rotation = rotation ?? oldSprite.Rotation;

				isDisposed = oldSprite.isDisposed;
			}
			else
			{
				Log.Error("couldn't construct Sprite: oldSprite is null");
				isDisposed = true;
			}
		}

		/// <summary>
		/// Releases the texture resource used by this <see cref="Sprite"/>.
		/// <para>When multiple <see cref="Sprite"/>s share the same texture file, the underlying resource map
		/// will only free the texture file when all of those <see cref="Sprite"/>s are disposed.</para>
		/// </summary>
		public void Dispose()
		{
			if(!isDisposed)
			{
				Core.Video.Textures.FreeTexture(textureID);
				isDisposed = true;
			}
			else
			{ Log.Warning("tried to Dispose of an already-disposed Sprite"); }
		}

		/// <summary>
		/// Draws this <see cref="Sprite"/> to the given <see cref="Window"/>.
		/// <para>This is functionally equivalent to calling <see cref="Window"/>.<see cref="Window.DrawSprite(Sprite)"/>
		/// with this <see cref="Sprite"/> as an argument.</para>
		/// </summary>
		/// <param name="window">The <see cref="Window"/> to which the <see cref="Sprite"/> should be drawn.</param>
		public void Draw(Window window)
		{
			if(!isDisposed)
			{ window.DrawTexture(textureID, Position, Rotation); }
			else
			{ Log.Error("couldn't draw Sprite: Sprite was disposed, or erroneously constructed"); }
		}
	};
}