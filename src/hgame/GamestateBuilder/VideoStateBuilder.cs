using System.Collections.Generic;
using heng;
using heng.Video;

namespace hgame
{
	public partial class GamestateBuilder
	{
		public class VideoStateBuilder
		{
			const int windowID = 0;

			readonly List<IDrawable> drawables;

			public VideoStateBuilder()
			{
				drawables = new List<IDrawable>();
			}

			public int AddDrawable(IDrawable drw)
			{
				Assert.Ref(drw);

				drawables.Add(drw);
				return drawables.Count - 1;
			}

			public VideoState Build(VideoState old)
			{
				Window w = old?.Windows[windowID] ?? BuildWindow();
				drawables.AddRange(DebugDrawSectors(5));

				return new VideoState(new Window[] { w }, drawables);
			}

			public void Clear()
			{
				drawables.Clear();
			}

			Window BuildWindow()
			{
				ScreenRect wRect = new ScreenRect(Window.Center, Window.Center, 640, 480);
				WindowFlags wFlags = WindowFlags.Shown;
				RendererFlags rFlags = RendererFlags.Accelerated | RendererFlags.PresentVSync;
				return new Window(windowID, "heng", wRect, wFlags, rFlags);
			}

			IEnumerable<IDrawable> DebugDrawSectors(int count)
			{
				for(int y = 0; y < count; y++)
				{
					WorldPoint start = new WorldPoint(0, y, Vector2.Zero);
					WorldPoint end = new WorldPoint(count, y, Vector2.Zero);
					yield return new LineDrawable(start, end, Color.Blue);
				}

				for(int x = 0; x < count; x++)
				{
					WorldPoint start = new WorldPoint(x, 0, Vector2.Zero);
					WorldPoint end = new WorldPoint(x, count, Vector2.Zero);
					yield return new LineDrawable(start, end, Color.Blue);
				}
			}
		};
	};
}