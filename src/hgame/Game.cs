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

			config.Events.EvLogMode = EventLogMode.Input;

			config.Audio.Format = heng.Audio.AudioFormat.S16;
			config.Audio.SampleRate = 44100;
			config.Audio.Channels = 2;
			config.Audio.Sounds.MaxSounds = 1024;
			config.Audio.Mixer.ChannelCount = 32;
			config.Audio.Mixer.AttenuationThreshold = 32;
			config.Audio.Mixer.StereoFalloffExponent = 0.15f;

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