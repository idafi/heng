using System;

namespace heng.Audio
{
	/// <summary>
	/// Represents a sound resource, loaded from file.
	/// <para>The underlying resource map will ensure sound resources using the same file will not
	/// incur duplicate file loads, if a <see cref="Sound"/> using the same file is already loaded.</para>
	/// Be sure to <see cref="Dispose"/> of the <see cref="Sound"/> when it's no longer needed.
	/// </summary>
	public class Sound : IDisposable
	{
		readonly int soundID;
		bool isDisposed;
		
		/// <summary>
		/// Creates a new <see cref="Sound"/> from the sound file at the given path.
		/// <para>A sound is just a loaded resource. To play it, send it through a <see cref="SoundSource"/>.</para>
		/// </summary>
		/// <param name="filePath">The file from which to create the new <see cref="Sound"/>.</param>
		public Sound(string filePath)
		{
			soundID = Core.Audio.Sounds.LoadSound(filePath);

			// core will take care of error logging
			if(soundID < 0)
			{ isDisposed = true; }
		}
		
		/// <inheritdoc />
		public void Dispose()
		{
			if(!isDisposed)
			{
				Core.Audio.Sounds.FreeSound(soundID);
				isDisposed = true;
			}
			else
			{ Log.Warning("tried to Dispose of an already-disposed Sound"); }
		}
		
		internal void PlayOn(int channel)
		{
			Assert.Index(channel, Core.Audio.Mixer.Channels.AUDIO_MIXER_CHANNELS_MAX);

			if(!isDisposed)
			{ Core.Audio.Mixer.Channels.SetSound(channel, soundID); }
			else
			{ Log.Error("couldn't play sound: sound was disposed, or erroneously constructed"); }
		}
	};
}