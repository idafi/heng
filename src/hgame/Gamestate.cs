﻿using heng;
using heng.Audio;
using heng.Input;
using heng.Physics;
using heng.Time;
using heng.Video;

namespace hgame
{
	public class Gamestate
	{
		public readonly PlayerUnit PlayerUnit;
		public readonly Unit Unit;
		public readonly Scenery Scenery;

		public readonly EventsState Events;
		public readonly InputState Input;
		public readonly PhysicsState Physics;
		public readonly VideoState Video;
		public readonly AudioState Audio;
		public readonly TimeState Time;
	
		public Gamestate(PlayerUnit playerUnit, Unit unit, Scenery scenery,
			EventsState events, InputState input, PhysicsState physics, VideoState video, AudioState audio, TimeState time)
		{
			Assert.Ref(playerUnit, unit, scenery, events, input, physics, video, audio, time);

			PlayerUnit = playerUnit;
			Scenery = scenery;
			Unit = unit;

			Events = events;
			Input = input;
			Physics = physics;
			Video = video;
			Audio = audio;
			Time = time;
		}
	};
}