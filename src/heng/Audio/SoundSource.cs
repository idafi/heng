using System.Collections.Generic;

namespace heng.Audio
{
	/// <summary>
	/// Represents a world-space source from which <see cref="Sound"/>s can be heard
	/// by the <see cref="AudioState"/>'s <see cref="AudioState.ListenerPosition"/>.
	/// </summary>
	public class SoundSource
	{
		/// <summary>
		/// The world-space position of this <see cref="SoundSource"/>.
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// All <see cref="SoundInstance"/>s currently playing from this <see cref="SoundSource"/>.
		/// <para>Expired instances will automatically be removed from this list when the <see cref="SoundSource"/>
		/// is constructed into the <see cref="AudioState"/>.</para>
		/// </summary>
		public readonly IReadOnlyList<SoundInstance> SoundInstances;
		
		/// <summary>
		/// Creates a new <see cref="SoundSource"/> at the given world-space position.
		/// </summary>
		/// <param name="position">The world-space position at which to locate the new <see cref="SoundSource"/>.</param>
		public SoundSource(WorldPoint position)
		{
			Position = position;
			SoundInstances = new SoundInstance[0];
		}
		
		SoundSource(WorldPoint position, IReadOnlyList<SoundInstance> instances)
		{
			Assert.Ref(instances);

			Position = position;
			SoundInstances = instances;
		}
		
		/// <summary>
		/// Moves the <see cref="SoundSource"/> to the given world-space position, preserving all
		/// <see cref="SoundInstance"/>s.
		/// </summary>
		/// <param name="position">The new position for the <see cref="SoundSource"/>.</param>
		/// <returns>A new <see cref="SoundSource"/> at the new position.</returns>
		public SoundSource Reposition(WorldPoint position)
		{
			return new SoundSource(position, SoundInstances);
		}
		
		/// <summary>
		/// Plays the given <see cref="Sound"/> on this <see cref="SoundSource"/>.
		/// <para>A new <see cref="SoundInstance"/> using this <see cref="Sound"/> will be created and internally
		/// managed. It'll then be automatically discarded when expired.</para>
		/// </summary>
		/// <param name="sound">The <see cref="Sound"/> from which to create a new <see cref="SoundInstance"/>.</param>
		/// <returns>A new <see cref="SoundSource"/>, playing the sound through a new <see cref="SoundInstance"/>.</returns>
		public SoundSource PlaySound(Sound sound)
		{
			if(sound != null)
			{
				List<SoundInstance> instances = new List<SoundInstance>(SoundInstances);
				instances.Add(new SoundInstance(sound));

				return new SoundSource(Position, instances);
			}
			
			return this;
		}

		internal SoundSource UpdateInstances(WorldPoint listenerPos, Core.Audio.Mixer.Channels.State channelState)
		{
			List<SoundInstance> newInstances = new List<SoundInstance>();
			foreach(SoundInstance instc in SoundInstances)
			{
				SoundInstance newInstc = instc.Update(channelState.Channels[instc.Channel]);

				if(newInstc.Progress < 1)
				{
					Vector2 newOffset = Position.PixelDistance(listenerPos);
					newInstc = newInstc.Reposition(newOffset);

					newInstances.Add(newInstc);
				}
			}

			return new SoundSource(Position, newInstances);
		}
	};
};