using heng.Input;
using heng.Physics;
using heng.Time;
using heng.Video;

namespace hgame
{
	public class Gamestate
	{
		public readonly InputState Input;
		public readonly PhysicsState Physics;
		public readonly VideoState Video;
		public readonly TimeState Time;

		public readonly PlayerUnit PlayerUnit;
		public readonly Scenery Scenery;
	
		public Gamestate(InputState input, PhysicsState physics, VideoState video, TimeState time,
			PlayerUnit playerUnit, Scenery scenery)
		{
			Input = input;
			Physics = physics;
			Video = video;
			Time = time;

			PlayerUnit = playerUnit;
			Scenery = scenery;
		}
	};
}