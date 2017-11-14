using heng;
using heng.Physics;
using heng.Video;

namespace hgame
{
	public class Unit
	{
		// pixels/s/s
		const float accel = 500f;

		readonly Texture texture;

		readonly int rigidBody;
		readonly int sprite;

		public Unit(GamestateBuilder newState)
		{
			Assert.Ref(newState);

			texture = new Texture("../../data/textest.bmp");

			WorldPoint pos = new WorldPoint((116, 80));

			Polygon collider = new Polygon(new Vector2(0, 0), new Vector2(32, 0), new Vector2(32, 32), new Vector2(0, 32));
			RigidBody body = new RigidBody(pos, new ConvexCollider(collider), 1f);
			Sprite spr = new Sprite(texture, pos, 0);

			rigidBody = newState.Physics.AddPhysicsObject(body);
			sprite = newState.Video.AddDrawable(spr);
		}

		Unit(Gamestate oldState, GamestateBuilder newState)
		{
			Unit oldUnit = oldState.Unit;
			texture = oldUnit.texture;

			RigidBody body = (RigidBody)(oldState.Physics.PhysicsObjects[oldUnit.rigidBody]);
			Sprite spr = (Sprite)(oldState.Video.Drawables[oldUnit.sprite]);

			rigidBody = newState.Physics.AddPhysicsObject(body);
			sprite = newState.Video.AddDrawable(spr.Reposition(body.Position));
		}

		public Unit Update(Gamestate oldState, GamestateBuilder newState)
		{
			return new Unit(oldState, newState);
		}
	};
}