using heng;
using heng.Audio;
using heng.Input;
using heng.Physics;
using heng.Video;

namespace hgame
{
	public class Scenery
	{
		readonly Sound sound;

		readonly int inputDevice;
		readonly int staticBody;
		readonly int rectDrawable;
		readonly int soundSource;

		public Scenery(GamestateBuilder newState)
		{
			Assert.Ref(newState);

			sound = new Sound("../../data/sto.ogg");

			InputDevice device = new InputDevice();
			device.RemapButton("SoundTest", new Key(KeyCode.Space));

			WorldPoint pos = new WorldPoint(new Vector2(276, 140));
			Rect rect = new Rect(Vector2.Zero, new Vector2(256, 16));

			Polygon collider = new Polygon(new Vector2(-256, -16), new Vector2(256, -16), new Vector2(256, 16), new Vector2(-256, 16));
			StaticBody body = new StaticBody(pos, new ConvexCollider(collider), PhysicsMaterialLibrary.Concrete);
			RectDrawable drw = new RectDrawable(rect, pos, true, Color.Black);
			SoundSource src = new SoundSource(body.Position);

			inputDevice = newState.Input.AddDevice(device);
			staticBody = newState.Physics.AddPhysicsObject(body);
			rectDrawable = newState.Video.AddDrawable(drw);
			soundSource = newState.Audio.AddSoundSource(src);
		}

		Scenery(Gamestate oldState, GamestateBuilder newState)
		{
			Assert.Ref(oldState, newState);

			Scenery oldScenery = oldState.Scenery;
			sound = oldScenery.sound;

			InputDevice device = oldState.Input.Devices[oldScenery.inputDevice];
			StaticBody body = (StaticBody)(oldState.Physics.PhysicsBodies[oldScenery.staticBody]);
			RectDrawable drw = (RectDrawable)(oldState.Video.Drawables[oldScenery.rectDrawable]);
			SoundSource src = oldState.Audio.SoundSources[oldScenery.soundSource];

			if(device.GetButtonPressed("SoundTest"))
			{ src = src.PlaySound(sound); }

			inputDevice = newState.Input.AddDevice(device);
			staticBody = newState.Physics.AddPhysicsObject(body);
			rectDrawable = newState.Video.AddDrawable(drw);
			soundSource = newState.Audio.AddSoundSource(src);
		}

		public Scenery Update(Gamestate oldState, GamestateBuilder newState)
		{
			return new Scenery(oldState, newState);
		}
	};
}