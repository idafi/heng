using heng;
using heng.Logging;

namespace hgame
{
	static class Game
	{
		static GamestateBuilder builder;
		static Gamestate gamestate;

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

				builder = new GamestateBuilder();
				gamestate = builder.Build(null);

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

			gamestate = builder.Build(gamestate);
		}

		static int Quit(int code)
		{
			Log.ClearLoggers();
			Engine.Quit();

			return code;
		}
	};
}