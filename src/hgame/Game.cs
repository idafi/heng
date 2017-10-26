using System;
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
			return false;
		}

		static void Frame()
		{
			time = new TimeState();
			video = new VideoState();

			time.DelayToTarget();
		}

		static int Quit(int code)
		{
			Log.ClearLoggers();
			Engine.Quit();

			Console.ReadLine();
			return code;
		}
	};
}