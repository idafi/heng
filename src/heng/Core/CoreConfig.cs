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

		/// <summary>
		/// Configuration settings for the engine core's audio system.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct AudioConfig
		{
			/// <summary>
			/// Configuration settings for the audio system's sound manager.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public struct SoundsConfig
			{
				/// <summary>
				/// The maximum number of sound resources that can be loaded.
				/// </summary>
				public int MaxSounds;
			};

			/// <summary>
			/// Configuration settings for the audio system's mixer.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public struct MixerConfig
			{
				/// <summary>
				/// The number of available mixing channels.
				/// </summary>
				public int ChannelCount;

				/// <summary>
				/// The minimum distance of a sound source before attenuation will apply.
				/// </summary>
				public float AttenuationThreshold;

				/// <summary>
				/// The exponent describing the speed with which multi-channel sounds will converge into a single channel.
				/// <para>I.e., a stereo sound will play from both channels when on top of the listener, but will converge
				/// into a mono-channel sound the farther away from the listener it goes.</para>
				/// </summary>
				public float StereoFalloffExponent;
			};

			/// <summary>
			/// The sample format at which audio should be played.
			/// <para>Sound resources in different formats will be converted to this target format when loaded.</para>
			/// </summary>
			public AudioFormat Format;

			/// <summary>
			/// The sample rate at which audio should be played.
			/// <para>Sound resources at different sample rates will be converted to the target rate when loaded.</para>
			/// </summary>
			public int SampleRate;

			/// <summary>
			/// The number of channels available to audio output.
			/// </summary>
			public int Channels;
			
			/// <summary>
			/// Configuration settings for the audio system's sound manager.
			/// </summary>
			public SoundsConfig Sounds;

			/// <summary>
			/// Configuration settings for the audio system's mixer.
			/// </summary>
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