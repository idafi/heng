﻿using System.Collections.Generic;
using heng;
using heng.Audio;

namespace hgame
{
	public partial class GamestateBuilder
	{
		public class AudioStateBuilder
		{
			readonly List<SoundSource> soundSources;
			readonly List<SoundInstance> soundInstances;
			WorldPoint listenerPos;

			public AudioStateBuilder()
			{
				soundSources = new List<SoundSource>();
				soundInstances = new List<SoundInstance>();
			}

			public int AddSoundSource(SoundSource source)
			{
				Assert.Ref(source);

				soundSources.Add(source);
				return soundSources.Count - 1;
			}

			public int AddSoundInstance(SoundInstance instc)
			{
				Assert.Ref(instc);

				soundInstances.Add(instc);
				return instc.ID;
			}

			public void SetListenerPosition(WorldPoint position)
			{
				listenerPos = position;
			}

			public AudioState Build(AudioState old)
			{
				if(old != null)
				{ soundInstances.AddRange(old.SoundInstances.Values); }

				return new AudioState(soundInstances, soundSources, listenerPos);
			}

			public void Clear()
			{
				soundSources.Clear();
				soundInstances.Clear();
			}
		};
	};
}