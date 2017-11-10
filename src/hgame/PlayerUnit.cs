using heng;
using heng.Audio;
using heng.Input;
using heng.Physics;
using heng.Video;

namespace hgame
{
	public class PlayerUnit
	{
		// pixels per second
		const float speed = 150;

		readonly Texture texture;
		readonly Sound sound;

		readonly int inputDevice;
		readonly int rigidBody;
		readonly int sprite;
		readonly int soundSource;
	
		public PlayerUnit(GamestateBuilder newState)
		{
			Assert.Ref(newState);

			texture = new Texture("../../data/textest.bmp");
			sound = new Sound("../../data/sto.ogg");

			InputDevice device = new InputDevice();
			device.RemapAxis("Horizontal", new ButtonAxis(new Key(KeyCode.Left), new Key(KeyCode.Right)));
			device.RemapAxis("Vertical", new ButtonAxis(new Key(KeyCode.Down), new Key(KeyCode.Up)));
			device.RemapButton("SoundTest", new Key(KeyCode.Space));
			
			WorldPoint pos = new WorldPoint(new Vector2(280 - 16, 100 - 16));

			Polygon collider = new Polygon(new Vector2(0, 0), new Vector2(32, 0), new Vector2(32, 32), new Vector2(0, 32));
			RigidBody body = new RigidBody(pos, new ConvexCollider(collider));
			Sprite spr = new Sprite(texture, pos, 0);
			SoundSource src = new SoundSource(body.Position);

			inputDevice = newState.Input.AddDevice(device);
			rigidBody = newState.Physics.AddRigidBody(body);
			sprite = newState.Video.AddDrawable(spr);
			soundSource = newState.Audio.AddSoundSource(src);

			newState.Video.SetCamera(new Camera(body.Position));
		}

		PlayerUnit(Gamestate oldState, GamestateBuilder newState)
		{
			Assert.Ref(oldState, newState);

			PlayerUnit oldUnit = oldState.PlayerUnit;
			texture = oldUnit.texture;
			sound = oldUnit.sound;

			InputDevice device = oldState.Input.Devices[oldUnit.inputDevice];
			RigidBody body = oldState.Physics.RigidBodies[oldUnit.rigidBody];
			Sprite spr = (Sprite)(oldState.Video.Drawables[oldUnit.sprite]);
			SoundSource src = oldState.Audio.SoundSources[oldUnit.soundSource];
		
			float x = device.GetAxisFrac("Horizontal");
			float y = device.GetAxisFrac("Vertical");
			float spd = speed * oldState.Time.Delta;
			Vector2 move = new Vector2(x, y).ClampMagnitude(0, 1) * spd;
			
			if(device.GetButtonPressed("SoundTest"))
			{
				SoundInstance instc = new SoundInstance(sound);

				int instcID = newState.Audio.AddSoundInstance(instc);
				src = src.PlaySound(instc.ID);
			}

			inputDevice = newState.Input.AddDevice(device);
			rigidBody = newState.Physics.AddRigidBody(body.AddImpulse(move));
			sprite = newState.Video.AddDrawable(spr.Reposition(body.Position));
			soundSource = newState.Audio.AddSoundSource(src.Reposition(body.Position));

			Window window = oldState.Video.Windows[0];
			Vector2 centerOffset = new Vector2(window.Rect.W / 2, window.Rect.H / 2);
			WorldPoint cameraPos = body.Position.PixelTranslate(-centerOffset);
			newState.Video.SetCamera(new Camera(cameraPos));
		}

		public PlayerUnit Update(Gamestate oldState, GamestateBuilder newState)
		{
			return new PlayerUnit(oldState, newState);
		}
	};
}