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

		readonly int inputDevice;
		readonly int rigidBody;
		readonly int sprite;
		readonly int soundSource;
	
		public PlayerUnit(GamestateBuilder newState)
		{
			Assert.Ref(newState);

			InputDevice device = new InputDevice();
			device.RemapAxis("Horizontal", new ButtonAxis(new Key(KeyCode.Left), new Key(KeyCode.Right)));
			device.RemapAxis("Vertical", new ButtonAxis(new Key(KeyCode.Down), new Key(KeyCode.Up)));
			device.RemapButton("SoundTest", new Key(KeyCode.Space));

			Polygon collider = new Polygon(new Vector2(0, 0), new Vector2(32, 0), new Vector2(32, 32), new Vector2(0, 32));
			RigidBody body = new RigidBody(new Vector2(280 - 16, 100 - 16), new ConvexCollider(collider));
			Sprite spr = new Sprite("../../data/textest.bmp", (ScreenPoint)(body.Position), 0);
			SoundSource src = new SoundSource(body.Position);

			inputDevice = newState.AddInputDevice(device);
			rigidBody = newState.AddRigidBody(body);
			sprite = newState.AddSprite(spr);
			soundSource = newState.AddSoundSource(src);
		}
	
		public PlayerUnit(Gamestate oldState, GamestateBuilder newState)
		{
			Assert.Ref(oldState, newState);

			PlayerUnit oldUnit = oldState.PlayerUnit;

			InputDevice device = oldState.Input.Devices[oldUnit.inputDevice];
			RigidBody body = oldState.Physics.RigidBodies[oldUnit.rigidBody];
			Sprite spr = oldState.Video.Sprites[oldUnit.sprite];
			SoundSource src = oldState.Audio.SoundSources[oldUnit.soundSource];
		
			float x = device.GetAxisFrac("Horizontal");
			float y = device.GetAxisFrac("Vertical");
			float spd = speed * oldState.Time.Delta;
			Vector2 move = new Vector2(x, y).ClampMagnitude(0, 1) * spd;
			
			inputDevice = newState.AddInputDevice(device);
			rigidBody = newState.AddRigidBody(body.AddImpulse(move));
			sprite = newState.AddSprite(new Sprite(spr, (ScreenPoint)(body.Position)));

			if(device.GetButtonPressed("SoundTest"))
			{
				Sound sound = new Sound("../../data/sto.ogg");
				SoundInstance instc = new SoundInstance(sound);

				int instcID = newState.AddSoundInstance(instc);
				src = src.PlaySound(instc.ID);
			}

			soundSource = newState.AddSoundSource(src.Reposition(body.Position));
		}
	};
}