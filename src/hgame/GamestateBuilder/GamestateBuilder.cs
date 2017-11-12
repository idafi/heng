using heng;
using heng.Audio;
using heng.Input;
using heng.Physics;
using heng.Time;
using heng.Video;

namespace hgame
{
	public partial class GamestateBuilder
	{
		public readonly EventStateBuilder Events;
		public readonly InputStateBuilder Input;
		public readonly PhysicsStateBuilder Physics;
		public readonly VideoStateBuilder Video;
		public readonly AudioStateBuilder Audio;
		public readonly TimeStateBuilder Time;

		public GamestateBuilder()
		{
			Events = new EventStateBuilder();
			Input = new InputStateBuilder();
			Physics = new PhysicsStateBuilder();
			Video = new VideoStateBuilder();
			Audio = new AudioStateBuilder();
			Time = new TimeStateBuilder();
		}

		public Gamestate Build(Gamestate old)
		{
			EventsState events = Events.Build();

			// we quit immediately on quit request, so don't bother building if we got one
			if(!events.IsQuitRequested)
			{
				PlayerUnit unit = old?.PlayerUnit.Update(old, this) ?? new PlayerUnit(this);
				Scenery scenery = old?.Scenery.Update(old, this) ?? new Scenery(this);

				InputState input = Input.Build();
				PhysicsState physics = Physics.Build(old?.Time.Delta ?? 0);
				VideoState video = Video.Build(old?.Video);
				AudioState audio = Audio.Build(old?.Audio);
				TimeState time = Time.Build(old?.Time);

				return new Gamestate(unit, scenery, events, input, physics, video, audio, time);
			}

			return new Gamestate(old.PlayerUnit, old.Scenery, events, old.Input, old.Physics, old.Video, old.Audio, old.Time);
		}

		public void Clear()
		{
			Input.Clear();
			Physics.Clear();
			Video.Clear();
			Audio.Clear();
		}
	};
}