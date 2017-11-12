using System.Collections.Generic;

namespace heng.Audio
{
	/// <summary>
	/// Represents an immutable snapshot of the audio system's current state.
	/// <para>Each frame, <see cref="Sound"/>s attached to <see cref="SoundSource"/>s -- via internally-managed
	/// <see cref="SoundInstance"/>s -- are automatically played, and attenuated relative to the <see cref="ListenerPosition"/>.</para>
	/// <see cref="SoundSource"/>s from previous <see cref="AudioState"/> instances need to be carried over to new state instnaces,
	/// or they won't continue playing! When doing so, finished sounds will automatically be culled from their parent <see cref="SoundSource"/>s.
	/// </summary>
	public class AudioState
	{
		/// <summary>
		/// All available <see cref="SoundSource"/>s.
		/// </summary>
		public readonly IReadOnlyList<SoundSource> SoundSources;

		/// <summary>
		/// The world-space position from which <see cref="SoundSource"/>s are heard.
		/// <para>Attenuation is calculated relative to the world-space position of the <see cref="SoundSource"/>s.</para>
		/// In most cases, the center of the camera is the best place for this.
		/// </summary>
		public readonly WorldPoint ListenerPosition;

		/// <summary>
		/// Constructs a new <see cref="AudioState"/>.
		/// </summary>
		/// <param name="sources">All <see cref="SoundSource"/>s available to the new state.</param>
		/// <param name="listener">The world-space position from which <see cref="SoundSource"/>s are heard.</param>
		public AudioState(IEnumerable<SoundSource> sources, WorldPoint listener)
		{
			ListenerPosition = listener;
			SoundSources = AddSources(sources);

			Core.Audio.PushSound();
		}

		IReadOnlyList<SoundSource> AddSources(IEnumerable<SoundSource> sources)
		{
			if(sources != null)
			{
				List<SoundSource> newSources = new List<SoundSource>();
				Core.Audio.GetSnapshot(out Core.Audio.State coreState);

				foreach(SoundSource source in sources)
				{
					if(source != null)
					{
						SoundSource newSource = source.UpdateInstances(ListenerPosition, coreState.Mixer.Channels);
						newSources.Add(newSource);
					}
					else
					{ Log.Error("couldn't add SoundSource to AudioState: source is null"); }
				}

				return newSources;
			}
			else
			{ Log.Warning("AudioState constructed with null sources collection"); }

			return new SoundSource[0];
		}
	};
}