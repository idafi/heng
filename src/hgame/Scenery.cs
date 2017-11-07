using heng;
using heng.Physics;
using heng.Video;

namespace hgame
{
	public class Scenery
	{
		readonly Texture texture;

		readonly int staticBody;
		readonly int sprite;

		public Scenery(GamestateBuilder newState)
		{
			Assert.Ref(newState);

			texture = new Texture("../../data/textest.bmp");

			WorldPoint pos = new WorldPoint(new Vector2(320 - 16, 240 - 16));

			Polygon collider = new Polygon(new Vector2(0, 0), new Vector2(32, 0), new Vector2(32, 32), new Vector2(0, 32));
			StaticBody body = new StaticBody(pos, new ConvexCollider(collider));
			Sprite spr = new Sprite(texture, pos, 0);

			staticBody = newState.Physics.AddStaticBody(body);
			sprite = newState.Video.AddDrawable(spr);
		}

		Scenery(Gamestate oldState, GamestateBuilder newState)
		{
			Assert.Ref(oldState, newState);

			Scenery oldScenery = oldState.Scenery;
			texture = oldScenery.texture;

			StaticBody body = oldState.Physics.StaticBodies[oldScenery.staticBody];
			Sprite spr = (Sprite)(oldState.Video.Drawables[oldScenery.sprite]);

			staticBody = newState.Physics.AddStaticBody(body);
			sprite = newState.Video.AddDrawable(spr);
		}

		public Scenery Update(Gamestate oldState, GamestateBuilder newState)
		{
			return new Scenery(oldState, newState);
		}
	};
}