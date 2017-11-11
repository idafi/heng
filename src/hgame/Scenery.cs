using heng;
using heng.Audio;
using heng.Input;
using heng.Physics;
using heng.Video;

namespace hgame
{
	public class Scenery
	{
		readonly Texture texture;
		readonly Sound sound;

		readonly int inputDevice;
		readonly int staticBody;
		readonly int sprite;
		readonly int soundSource;

		public Scenery(GamestateBuilder newState)
		{
			Assert.Ref(newState);

			texture = new Texture("../../data/textest.bmp");
			sound = new Sound("../../data/sto.ogg");

			InputDevice device = new InputDevice();
			device.RemapButton("SoundTest", new Key(KeyCode.Space));

			WorldPoint pos = new WorldPoint(new Vector2(340 - 16, 260 - 16));

			Polygon collider = new Polygon(new Vector2(0, 0), new Vector2(32, 0), new Vector2(32, 32), new Vector2(0, 32));
			StaticBody body = new StaticBody(pos, new ConvexCollider(collider));
			Sprite spr = new Sprite(texture, pos, 0);
			SoundSource src = new SoundSource(body.Position);

			inputDevice = newState.Input.AddDevice(device);
			staticBody = newState.Physics.AddPhysicsObject(body);
			sprite = newState.Video.AddDrawable(spr);
			soundSource = newState.Audio.AddSoundSource(src);
		}

		Scenery(Gamestate oldState, GamestateBuilder newState)
		{
			Assert.Ref(oldState, newState);

			Scenery oldScenery = oldState.Scenery;
			texture = oldScenery.texture;
			sound = oldScenery.sound;

			InputDevice device = oldState.Input.Devices[oldScenery.inputDevice];
			StaticBody body = (StaticBody)(oldState.Physics.PhysicsObjects[oldScenery.staticBody]);
			Sprite spr = (Sprite)(oldState.Video.Drawables[oldScenery.sprite]);
			SoundSource src = oldState.Audio.SoundSources[oldScenery.soundSource];

			if(device.GetButtonPressed("SoundTest"))
			{ src = src.PlaySound(sound); }

			inputDevice = newState.Input.AddDevice(device);
			staticBody = newState.Physics.AddPhysicsObject(body);
			sprite = newState.Video.AddDrawable(spr);
			soundSource = newState.Audio.AddSoundSource(src);
		}

		public Scenery Update(Gamestate oldState, GamestateBuilder newState)
		{
			return new Scenery(oldState, newState);
		}
	};
}