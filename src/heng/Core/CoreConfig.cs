using System.Runtime.InteropServices;
using heng.Audio;
using heng.Logging;

namespace heng
{
	/// <summary>
	/// Configuration settings for the <see cref="Engine"/> core.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CoreConfig
	{
		/// <summary>
		/// Configuration settings for the engine core's logging system.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct LogConfig
		{
			/// <summary>
			/// The engine core won't print messages less severe than this to the console.
			/// </summary>
			public LogLevel MinLevelConsole;

			/// <summary>
			/// The engine core won't print messages less severe than this to the output file.
			/// </summary>
			public LogLevel MinLevelFile;
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct AudioConfig
		{
			[StructLayout(LayoutKind.Sequential)]
			public struct SoundsConfig
			{
				public int MaxSounds;
			};

			[StructLayout(LayoutKind.Sequential)]
			public struct MixerConfig
			{
				public int ChannelCount;
				public int AttenuationThreshold;
				public float StereoFalloffExponent;
			};

			public AudioFormat Format;
			public int SampleRate;
			public int Channels;
			
			public SoundsConfig Sounds;
			public MixerConfig Mixer;
		};

		/// <summary>
		/// Configuration settings for the engine core's logging system.
		/// </summary>
		public LogConfig Log;

		/// <summary>
		/// Configuration settings for the engine core's audio system.
		/// </summary>
		public AudioConfig Audio;
	};
}