﻿using System.Collections.Generic;
using heng;
using heng.Audio;
using heng.Input;
using heng.Physics;
using heng.Time;
using heng.Video;

namespace hgame
{
	public class GamestateBuilder
	{
		readonly List<InputDevice> inputDevices;

		readonly List<RigidBody> rigidBodies;
		readonly List<StaticBody> staticBodies;
		
		readonly List<Sprite> sprites;

		readonly List<SoundSource> soundSources;
		readonly List<SoundInstance> soundInstances;

		public GamestateBuilder()
		{
			inputDevices = new List<InputDevice>();

			rigidBodies = new List<RigidBody>();
			staticBodies = new List<StaticBody>();

			sprites = new List<Sprite>();

			soundSources = new List<SoundSource>();
			soundInstances = new List<SoundInstance>();
		}

		public int AddInputDevice(InputDevice device)
		{
			Assert.Ref(device);

			inputDevices.Add(device);
			return inputDevices.Count - 1;
		}

		public int AddRigidBody(RigidBody body)
		{
			Assert.Ref(body);

			rigidBodies.Add(body);
			return rigidBodies.Count - 1;
		}

		public int AddStaticBody(StaticBody body)
		{
			Assert.Ref(body);

			staticBodies.Add(body);
			return staticBodies.Count - 1;
		}

		public int AddSprite(Sprite spr)
		{
			Assert.Ref(spr);

			sprites.Add(spr);
			return sprites.Count - 1;
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

		public Gamestate Build(Gamestate old)
		{
			EventsState events = new EventsState();

			// we quit immediately when requested, so don't let the rest of the frame muck that up
			if(!events.IsQuitRequested)
			{
				PlayerUnit playerUnit = BuildPlayerUnit(old);
				Scenery scenery = BuildScenery(old);
				Window window = BuildWindow(old);

				Gamestate g = BuildGamestate(old, events, playerUnit, scenery, window);

				Clear();
				return g;
			}
			else
			{ return new Gamestate(old.PlayerUnit, old.Scenery, events, old.Input, old.Physics, old.Video, old.Audio, old.Time); }
		}

		PlayerUnit BuildPlayerUnit(Gamestate old)
		{
			return (old != null) ? new PlayerUnit(old, this) : new PlayerUnit(this);
		}

		Scenery BuildScenery(Gamestate old)
		{
			return (old != null) ? new Scenery(old, this) : new Scenery(this);
		}

		Window BuildWindow(Gamestate old)
		{
			const int windowID = 0;

			if(old == null)
			{
				ScreenRect wRect = new ScreenRect(Window.Center, Window.Center, 640, 480);
				WindowFlags wFlags = WindowFlags.Shown;
				RendererFlags rFlags = RendererFlags.Accelerated | RendererFlags.PresentVSync;
				return new Window(windowID, "heng", wRect, wFlags, rFlags);
			}
			else
			{ return old.Video.Windows[windowID]; }
		}

		// TODO: what a nightmarish signature. all of this should be cleaned soon
		Gamestate BuildGamestate(Gamestate old, EventsState events, PlayerUnit playerUnit, Scenery scenery, Window window)
		{
			Assert.Ref(playerUnit, scenery, window);

			if(old != null)
			{ soundInstances.AddRange(old.Audio.SoundInstances.Values); }

			InputState input = new InputState(inputDevices);
			PhysicsState physics = new PhysicsState(rigidBodies, staticBodies);
			VideoState video = new VideoState(new Window[] { window }, sprites);
			AudioState audio = new AudioState(soundInstances, soundSources, new Vector2(320, 240));
			TimeState time = old?.Time.Update(0) ?? new TimeState();

			return new Gamestate(playerUnit, scenery, events, input, physics, video, audio, time);
		}

		void Clear()
		{
			inputDevices.Clear();

			rigidBodies.Clear();
			staticBodies.Clear();

			sprites.Clear();

			soundSources.Clear();
			soundInstances.Clear();
		}
	};
}
