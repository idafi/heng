using System.Collections.Generic;

namespace heng.Audio
{
	/// <summary>
	/// Represents a pixel-space source from which <see cref="SoundInstance"/>s can be heard
	/// by the <see cref="AudioState"/>'s <see cref="AudioState.ListenerPosition"/>.
	/// </summary>
	public class SoundSource
	{
		/// <summary>
		/// The world-space positon of this <see cref="SoundSource"/>.
		/// </summary>
		public readonly WorldPoint Position;

		/// <summary>
		/// All <see cref="SoundInstance"/> IDs currently playing from this <see cref="SoundSource"/>.
		/// <para>The <see cref="AudioState"/> will automatically remove expired instances from this
		/// list when constructed.</para>
		/// </summary>
		public readonly IReadOnlyList<int> SoundInstances;
		
		/// <summary>
		/// Creates a new <see cref="SoundSource"/> at the given pixel-space position.
		/// </summary>
		/// <param name="position">The world-space position at which to locate the new <see cref="SoundSource"/>.</param>
		public SoundSource(WorldPoint position)
		{
			Position = position;
			SoundInstances = new int[0];
		}
		
		SoundSource(WorldPoint position, IReadOnlyList<int> instances)
		{
			Assert.Ref(instances);

			Position = position;
			SoundInstances = instances;
		}
		
		/// <summary>
		/// Moves the <see cref="SoundSource"/> to the given world-space position, preserving all
		/// sound instances.
		/// </summary>
		/// <param name="position">The new position for the <see cref="SoundSource"/>.</param>
		/// <returns>A new <see cref="SoundSource"/> at the new position.</returns>
		public SoundSource Reposition(WorldPoint position)
		{
			return new SoundSource(position, SoundInstances);
		}
		
		/// <summary>
		/// Attaches a <see cref="SoundInstance"/> <see cref="SoundInstance.ID"/> to this <see cref="SoundSource"/>, allowing it
		/// to be played and automatically attuned from the <see cref="SoundSource"/>'s <see cref="Position"/>.
		/// </summary>
		/// <param name="instance">The <see cref="SoundInstance"/>'s unique <see cref="SoundInstance.ID"/>.</param>
		/// <returns>A new <see cref="SoundSource"/>, with the new instance's ID attached.</returns>
		public SoundSource PlaySound(int instance)
		{
			int[] instances = new int[SoundInstances.Count + 1];
			instances[instances.Length - 1] = instance;
			
			return new SoundSource(Position, instances);
		}
		
		/// <summary>
		/// Stops a currently playing <see cref="SoundInstance"/>, detaching it from this <see cref="SoundSource"/>.
		/// <para>If the same <see cref="SoundInstance"/> is still playing on another <see cref="SoundSource"/> for some
		/// reason, it will continue to do so.</para>
		/// </summary>
		/// <param name="instance">The unique <see cref="SoundInstance.ID"/> of the <see cref="SoundInstance"/> to detach.</param>
		/// <returns>A new <see cref="SoundSource"/>, with the instance's ID detached.</returns>
		public SoundSource StopSound(int instance)
		{
			List<int> instances = new List<int>(SoundInstances);
			instances.Remove(instance);
			
			return new SoundSource(Position, instances);
		}
	};
};