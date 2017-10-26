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

				time = new TimeState();
				time.TargetFrametime = 16;

				video = new VideoState();

				ScreenRect rect = new ScreenRect(805240832, 805240832, 640, 480);
				WindowFlags wFlags = WindowFlags.Shown;
				RendererFlags rFlags = RendererFlags.Accelerated | RendererFlags.PresentVSync;
				video.OpenWindow("heng", rect, wFlags, rFlags);

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
			time = new TimeState();
			Engine.PumpEvents();

			video = new VideoState();

			ScreenPoint[] points =
			{
				new ScreenPoint(320, 240),
				new ScreenPoint(160, 120),
				new ScreenPoint(480, 360)
			};

			video.Windows[0].Clear(Color.White);
			video.Windows[0].DrawPoints(points, Color.Blue);
			video.Windows[0].Present();

			time.DelayToTarget();
		}

		static int Quit(int code)
		{
			Log.ClearLoggers();
			Engine.Quit();

			return code;
		}
	};
}