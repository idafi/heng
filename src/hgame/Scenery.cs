using heng;
using heng.Physics;
using heng.Video;

namespace hgame
{
	public class Scenery
	{
		readonly int staticBody;
		readonly int sprite;

		public Scenery(GamestateBuilder newState)
		{
			Polygon collider = new Polygon(new Vector2(0, 0), new Vector2(32, 0), new Vector2(32, 32), new Vector2(0, 32));
			StaticBody body = new StaticBody(new Vector2(320 - 16, 240 - 16), new ConvexCollider(collider));
			Sprite spr = new Sprite("../../data/textest.bmp", (ScreenPoint)(body.Position), 0);

			staticBody = newState.AddStaticBody(body);
			sprite = newState.AddSprite(spr);
		}

		public Scenery(Gamestate oldState, GamestateBuilder newState)
		{
			Scenery oldScenery = oldState.Scenery;
			StaticBody body = oldState.Physics.StaticBodies[oldScenery.staticBody];
			Sprite spr = oldState.Video.Sprites[oldScenery.sprite];

			staticBody = newState.AddStaticBody(body);
			sprite = newState.AddSprite(spr);
		}
	};
}