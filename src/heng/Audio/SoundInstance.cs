namespace heng.Audio
{
	/// <summary>
	/// Represents a paused or playing instance of a <see cref="heng.Audio.Sound"/> resource.
	/// <para><see cref="SoundInstance"/>s are used with <see cref="SoundSource"/>s to construct
	/// the <see cref="AudioState"/> for a frame. The <see cref="SoundInstance"/> thus provides a unique
	/// <see cref="ID"/>, allowing you to read and carry over the <see cref="SoundInstance"/> from a previous
	/// frame's <see cref="AudioState"/>.</para>
	/// Once the <see cref="SoundInstance"/> has finished playing, the <see cref="AudioState"/> will automatically
	/// filter it out when constructing the frame. Futher operations on the same <see cref="ID"/> will no longer be valid.
	/// </summary>
	public class SoundInstance
	{
		static int nextID;

		/// <summary>
		/// The unique ID for this <see cref="SoundInstance"/>.
		/// <para>This is a key used to retrieve this <see cref="SoundInstance"/> from the <see cref="AudioState"/>'s
		/// <see cref="AudioState.SoundInstances"/> collection.</para>
		/// </summary>
		public readonly int ID;
		
		/// <summary>
		/// The <see cref="Sound"/> resource from which this <see cref="SoundInstance"/> was created.
		/// </summary>
		public readonly Sound Sound;

		/// <summary>
		/// The mixer channel on which this sound is playing.
		/// </summary>
		public readonly int Channel;
		
		/// <summary>
		/// A 0-1 fraction representing how much of the <see cref="Sound"/> resource has been played.
		/// </summary>
		public readonly float Progress;

		/// <summary>
		/// The <see cref="SoundInstance"/>'s position relative to the <see cref="AudioState"/>'s
		/// <see cref="AudioState.ListenerPosition"/>.
		/// <para>This is meant to be set by the <see cref="AudioState"/> through a <see cref="SoundSource"/>,
		/// to which the <see cref="SoundInstance"/> is attached.</para>
		/// </summary>
		public readonly Vector2 ListenerOffset;
		
		/// <summary>
		/// Creates a new <see cref="SoundInstance"/> using the given <see cref="Sound"/> resource.
		/// </summary>
		/// <param name="sound">The <see cref="Sound"/> resource used to construct the new <see cref="SoundInstance"/>.</param>
		public SoundInstance(Sound sound)
		{
			if(sound != null)
			{
				Channel = Core.Audio.Mixer.Channels.GetNextFreeChannel();
				if(Channel > -1)
				{
					ID = nextID++;
					Sound = sound;

					Progress = 0;
					ListenerOffset = Vector2.Zero;

					Sound.PlayOn(Channel);
				}
				else
				{ Log.Error("couldn't construct SoundInstance: no free mixer channels"); }
			}
			else
			{ Log.Error("couldn't construct SoundInstance: source Sound is null"); }
		}
		
		SoundInstance(int id, Sound sound, int channel, float progress, Vector2 offset)
		{
			Assert.Ref(sound);
			Assert.Index(channel, Core.Audio.Mixer.Channels.AUDIO_MIXER_CHANNELS_MAX);

			ID = id;
			
			Sound = sound;
			Channel = channel;
			
			Progress = progress;
			ListenerOffset = offset;
		}
		
		/// <summary>
		/// Repositions the <see cref="SoundInstance"/>'s listener offset.
		/// </summary>
		/// <param name="offset">The pixel-space offset of this <see cref="SoundInstance"/> relative to the
		/// <see cref="AudioState"/>'s <see cref="AudioState.ListenerPosition"/>.</param>
		/// <returns>A new <see cref="SoundInstance"/>, identical to this, repositioned at the new offset.</returns>
		public SoundInstance Reposition(Vector2 offset)
		{
			return new SoundInstance(ID, Sound, Channel, Progress, offset);
		}
		
		internal SoundInstance Update(Core.Audio.Mixer.Channels.MixerChannel channelState)
		{
			float progress = (float)(channelState.DataPos) / (float)(channelState.DataLen);

			if(progress < 1)
			{
				Core.Audio.Mixer.Channels.CalcAttenuation(Channel, ListenerOffset, 300);
				Core.Audio.Mixer.Channels.Advance(Channel, false);
			}
			else
			{ Core.Audio.Mixer.Channels.SetSound(Channel, -1); }

			return new SoundInstance(ID, Sound, Channel, progress, ListenerOffset);
		}
	};
}
