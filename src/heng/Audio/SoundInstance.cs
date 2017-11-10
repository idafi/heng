namespace heng.Audio
{
	/// <summary>
	/// Represents a paused or playing instance of a <see cref="heng.Audio.Sound"/> resource.
	/// <para><see cref="SoundInstance"/>s are internally managed by <see cref="SoundSource"/>s and the
	/// <see cref="AudioState"/>. You can't manipulate them yourself, but you can still read relevant
	/// data, such as their total <see cref="Progress"/>.</para>
	/// Once the <see cref="SoundInstance"/> has finished playing, the <see cref="AudioState"/> will automatically
	/// filter it out when constructing its parent <see cref="SoundSource"/>.
	/// </summary>
	public class SoundInstance
	{
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
		
		internal SoundInstance(Sound sound)
		{
			Assert.Ref(sound);

			Channel = Core.Audio.Mixer.Channels.GetNextFreeChannel();
			if(Channel > -1)
			{
				Sound = sound;

				Progress = 0;
				ListenerOffset = Vector2.Zero;

				Sound.PlayOn(Channel);
			}
			else
			{ Log.Error("couldn't construct SoundInstance: no free mixer channels"); }
		}
		
		SoundInstance(Sound sound, int channel, float progress, Vector2 offset)
		{
			Assert.Ref(sound);
			Assert.Index(channel, Core.Audio.Mixer.Channels.AUDIO_MIXER_CHANNELS_MAX);
			
			Sound = sound;
			Channel = channel;
			
			Progress = progress;
			ListenerOffset = offset;
		}
		
		internal SoundInstance Reposition(Vector2 offset)
		{
			return new SoundInstance(Sound, Channel, Progress, offset);
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

			return new SoundInstance(Sound, Channel, progress, ListenerOffset);
		}
	};
}
