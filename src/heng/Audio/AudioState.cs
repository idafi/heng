using System.Collections.Generic;

namespace heng.Audio
{
	/// <summary>
	/// Represents an immutable snapshot of the audio system's current state.
	/// <para>Each frame, <see cref="SoundInstance"/>s are attached to <see cref="SoundSource"/>s,
	/// which are then automatically played, and attenuated relative to the <see cref="ListenerPosition"/>.</para>
	/// Each extant <see cref="SoundInstance"/> should be carried over to new <see cref="AudioState"/> snapshots;
	/// any instances that expire will then automatically be culled from the new state.
	/// </summary>
	public class AudioState
	{
		/// <summary>
		/// All <see cref="SoundInstance"/>s currently paused or playing.
		/// </summary>
		public readonly IReadOnlyDictionary<int, SoundInstance> SoundInstances;

		/// <summary>
		/// All available <see cref="SoundSource"/>s.
		/// </summary>
		public readonly IReadOnlyList<SoundSource> SoundSources;

		/// <summary>
		/// The pixel-space position from which <see cref="SoundSource"/>s are heard.
		/// <para>Attenuation is calculated relative to the pixel-space position of the <see cref="SoundSource"/>s.</para>
		/// In most cases, the center of the camera is the best place for this.
		/// </summary>
		public readonly Vector2 ListenerPosition;

		/// <summary>
		/// Constructs a new <see cref="AudioState"/> using the given elements.
		/// </summary>
		/// <param name="instances">
		/// All <see cref="SoundInstance"/>s currently paused or playing.
		/// <para>Don't forget to include the last <see cref="AudioState"/>'s <see cref="SoundInstance"/>s, or they
		/// won't continue playing as the new state is constructed.</para>
		/// </param>
		/// <param name="sources">All <see cref="SoundSource"/>s available to the new state.</param>
		/// <param name="listener">The pixel-space position from which <see cref="SoundSource"/>s are heard.</param>
		public AudioState(IEnumerable<SoundInstance> instances, IEnumerable<SoundSource> sources, Vector2 listener)
		{
			List<SoundSource> newSources = new List<SoundSource>();
			Dictionary<int, SoundInstance> newInstances = new Dictionary<int, SoundInstance>();
			ListenerPosition = listener;

			Core.Audio.GetSnapshot(out Core.Audio.State coreState);

			foreach(SoundInstance instc in instances)
			{
				ref Core.Audio.Mixer.Channels.MixerChannel ch = ref coreState.Mixer.Channels.Channels[instc.Channel];
				SoundInstance newInstc = instc.Update(ch);

				if(newInstc.Progress < 1)
				{ newInstances.Add(newInstc.ID, newInstc); }
			}

			foreach(SoundSource source in sources)
			{
				SoundSource newSource = source;

				foreach(int instanceID in source.SoundInstances)
				{
					if(newInstances.TryGetValue(instanceID, out SoundInstance instc))
					{ newInstances[instanceID] = instc.Reposition(source.Position - ListenerPosition); }
					else
					{ newSource = source.StopSound(instanceID); }
				}

				newSources.Add(source);
			}

			SoundSources = newSources;
			SoundInstances = newInstances;

			Core.Audio.PushSound();
		}
	};
}