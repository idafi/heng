using heng;
using heng.Logging;
using heng.Time;
using heng.Video;

namespace hgame
{
	static class Game
	{
		static VideoState video;
		static TimeState time;

		static int Main(string[] args)
		{
			if(Init())
			{
				while(!ShouldQuit())
				{ Frame(); }

				return Quit(0);
			}

			return Quit(1);
		}

		static bool Init()
		{
			CoreConfig config = new CoreConfig();
			config.Log.MinLevelConsole = LogLevel.Debug;
			config.Log.MinLevelFile = LogLevel.Warning;

			if(Engine.Init(config))
			{
				Log.AddLogger(new ConsoleLogger(), LogLevel.Debug);
				Log.AddLogger(new FileLogger(), LogLevel.Warning);

				time = new TimeState(0, 0);

				return true;
			}

			return false;
		}

		static bool ShouldQuit()
		{
			return Engine.IsQuitRequested();
		}

		static void Frame()
		{
			Engine.PumpEvents();

			ScreenPoint[] points =
			{
				new ScreenPoint(320, 240),
				new ScreenPoint(160, 120),
				new ScreenPoint(480, 360)
			};

			ScreenRect wRect = new ScreenRect(805240832, 805240832, 640, 480);
			WindowFlags wFlags = WindowFlags.Shown;
			RendererFlags rFlags = RendererFlags.Accelerated | RendererFlags.PresentVSync;

			Sprite spr = new Sprite("../../data/textest.bmp", new ScreenPoint(320 - 16, 240 - 16), 0);

			Window w = new Window(0, "heng", wRect, wFlags, rFlags, Color.White);
			w.DrawPoints(points, Color.Blue);
			spr.Draw(w);

			video = new VideoState(new Window[] { w });
			time = new TimeState(time.TotalTicks, 16);
		}

		static int Quit(int code)
		{
			Log.ClearLoggers();
			Engine.Quit();

			return code;
		}
	};
}