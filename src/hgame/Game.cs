using System;
using heng;
using heng.Logging;
using heng.Video;

namespace hgame
{
	static class Game
	{
		static VideoState video;

		static int Main(string[] args)
		{
			if(Init())
			{
				CoreTester tester = new CoreTester();
				tester.Test();

				Log.Debug("hgame: psst");
				Log.Note("hgame: hello");
				Log.Warning("hgame: uh");
				Log.Error("hgame: oh no");
				Log.Failure("hgame: whoops");

				Console.ReadLine();
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

				video = new VideoState();

				ScreenRect rect = new ScreenRect(805240832, 805240832, 640, 480);
				WindowFlags wFlags = WindowFlags.Shown;
				RendererFlags rFlags = RendererFlags.Accelerated | RendererFlags.PresentVSync;
				video.OpenWindow("heng", rect, wFlags, rFlags);

				video = new VideoState();

				return true;
			}

			return false;
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