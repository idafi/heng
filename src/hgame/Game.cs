using heng;
using heng.Input;
using heng.Logging;
using heng.Time;
using heng.Video;

namespace hgame
{
	static class Game
	{
		const int windowID = 0;
		const float speed = 150;	// pixels per second

		static InputDevice device;
		static Sprite sprite;
		static ScreenPoint[] points;
		
		static InputState input;
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

				input = new InputState();
				device = new InputDevice(input, input);
				device.RemapAxis("Horizontal", new ButtonAxis(new Key(KeyCode.Left), new Key(KeyCode.Right)));
				device.RemapAxis("Vertical", new ButtonAxis(new Key(KeyCode.Down), new Key(KeyCode.Up)));

				ScreenRect wRect = new ScreenRect(805240832, 805240832, 640, 480);
				WindowFlags wFlags = WindowFlags.Shown;
				RendererFlags rFlags = RendererFlags.Accelerated | RendererFlags.PresentVSync;
				Window w = new Window(windowID, "heng", wRect, wFlags, rFlags);

				sprite = new Sprite("../../data/textest.bmp", new ScreenPoint(320 - 16, 240 - 16), 0);

				points = new ScreenPoint[]
				{
					new ScreenPoint(320, 240),
					new ScreenPoint(160, 120),
					new ScreenPoint(480, 360)
				};

				video = new VideoState(new Window[] { w });
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

			input = new InputState();
			device = new InputDevice(device, input);

			float x = device.GetAxisFrac("Horizontal");
			float y = device.GetAxisFrac("Vertical");
			float spd = speed * time.Delta;
			Vector2 move = new Vector2(x, y).ClampMagnitude(0, 1) * spd;
			
			sprite = new Sprite(sprite, sprite.Position + (ScreenPoint)(move));

			Window w = new Window(video.Windows[windowID]);
			w.Clear(Color.White);
			w.DrawPoints(points, Color.Blue);
			w.DrawSprite(sprite);

			video = new VideoState(new Window[] { w });
			time = new TimeState(time.TotalTicks, 0);
		}

		static int Quit(int code)
		{
			Log.ClearLoggers();
			Engine.Quit();

			return code;
		}
	};
}