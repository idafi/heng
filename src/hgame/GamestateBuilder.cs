using System.Collections.Generic;
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
	
		public GamestateBuilder()
		{
			inputDevices = new List<InputDevice>();

			rigidBodies = new List<RigidBody>();
			staticBodies = new List<StaticBody>();

			sprites = new List<Sprite>();
		}

		public int AddInputDevice(InputDevice device)
		{
			inputDevices.Add(device);
			return inputDevices.Count - 1;
		}

		public int AddRigidBody(RigidBody body)
		{
			rigidBodies.Add(body);
			return rigidBodies.Count - 1;
		}

		public int AddStaticBody(StaticBody body)
		{
			staticBodies.Add(body);
			return staticBodies.Count - 1;
		}

		public int AddSprite(Sprite spr)
		{
			sprites.Add(spr);
			return sprites.Count - 1;
		}
	
		public Gamestate Build(Gamestate old)
		{
			PlayerUnit playerUnit;
			Scenery scenery;
			Window window;

			if(old == null)
			{
				playerUnit = new PlayerUnit(this);
				scenery = new Scenery(this);

				ScreenRect wRect = new ScreenRect(805240832, 805240832, 640, 480);
				WindowFlags wFlags = WindowFlags.Shown;
				RendererFlags rFlags = RendererFlags.Accelerated | RendererFlags.PresentVSync;
				window = new Window(0, "heng", wRect, wFlags, rFlags);
			}
			else
			{
				playerUnit = new PlayerUnit(old, this);
				scenery = new Scenery(old, this);

				window = old.Video.Windows[0];
			}

			InputState input = new InputState(inputDevices);
			PhysicsState physics = new PhysicsState(rigidBodies, staticBodies);
			VideoState video = new VideoState(new Window[] { window }, sprites);
			TimeState time = new TimeState((old != null) ? old.Time.TotalTicks : 0, 0);

			inputDevices.Clear();

			rigidBodies.Clear();
			staticBodies.Clear();

			sprites.Clear();

			return new Gamestate(playerUnit, scenery, input, physics, video, time);
		}
	};
}
