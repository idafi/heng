using System.Collections.Generic;
using heng;
using heng.Audio;

namespace hgame
{
	public partial class GamestateBuilder
	{
		public class AudioStateBuilder
		{
			readonly List<SoundSource> soundSources;
			WorldPoint listenerPos;

			public AudioStateBuilder()
			{
				soundSources = new List<SoundSource>();
			}

			public int AddSoundSource(SoundSource source)
			{
				Assert.Ref(source);

				soundSources.Add(source);
				return soundSources.Count - 1;
			}

			public void SetListenerPosition(WorldPoint position)
			{
				listenerPos = position;
			}

			public AudioState Build(AudioState old)
			{
				return new AudioState(soundSources, listenerPos);
			}

			public void Clear()
			{
				soundSources.Clear();
			}
		};
	};
}