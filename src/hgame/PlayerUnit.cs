using heng;
using heng.Input;
using heng.Physics;
using heng.Video;

namespace hgame
{
	public class PlayerUnit
	{
		// pixels/s/s
		const float accel = 150f;

		readonly Texture texture;

		readonly int inputDevice;
		readonly int rigidBody;
		readonly int sprite;
	
		public PlayerUnit(GamestateBuilder newState)
		{
			Assert.Ref(newState);

			texture = new Texture("../../data/textest.bmp");

			InputDevice device = new InputDevice();
			device.RemapAxis("Horizontal", new ButtonAxis(new Key(KeyCode.Left), new Key(KeyCode.Right)));
			device.RemapAxis("Vertical", new ButtonAxis(new Key(KeyCode.Down), new Key(KeyCode.Up)));
			
			WorldPoint pos = new WorldPoint(new Vector2(280 - 16, 100 - 16));

			Polygon collider = new Polygon(new Vector2(0, 0), new Vector2(32, 0), new Vector2(32, 32), new Vector2(0, 32));
			RigidBody body = new RigidBody(pos, new ConvexCollider(collider), 1f);
			Sprite spr = new Sprite(texture, pos, 0);

			inputDevice = newState.Input.AddDevice(device);
			rigidBody = newState.Physics.AddPhysicsObject(body);
			sprite = newState.Video.AddDrawable(spr);
			newState.Audio.SetListenerPosition(body.Position);

			newState.Video.SetCamera(new Camera(body.Position));
		}

		PlayerUnit(Gamestate oldState, GamestateBuilder newState)
		{
			Assert.Ref(oldState, newState);

			PlayerUnit oldUnit = oldState.PlayerUnit;
			texture = oldUnit.texture;

			InputDevice device = oldState.Input.Devices[oldUnit.inputDevice];
			RigidBody body = (RigidBody)(oldState.Physics.PhysicsObjects[oldUnit.rigidBody]);
			Sprite spr = (Sprite)(oldState.Video.Drawables[oldUnit.sprite]);

			float x = device.GetAxisFrac("Horizontal");
			float y = device.GetAxisFrac("Vertical");
			Vector2 move = new Vector2(x, y).ClampMagnitude(0, 1) * accel;

			inputDevice = newState.Input.AddDevice(device);
			rigidBody = newState.Physics.AddPhysicsObject(body.AddImpulse(move));
			sprite = newState.Video.AddDrawable(spr.Reposition(body.Position));
			newState.Audio.SetListenerPosition(body.Position);

			Window window = oldState.Video.Windows[0];
			Vector2 centerOffset = new Vector2(window.Rect.W / 2, window.Rect.H / 2);
			WorldPoint cameraPos = body.Position.PixelTranslate(-centerOffset + new Vector2(16, 16));
			newState.Video.SetCamera(new Camera(cameraPos));
		}

		public PlayerUnit Update(Gamestate oldState, GamestateBuilder newState)
		{
			return new PlayerUnit(oldState, newState);
		}
	};
}