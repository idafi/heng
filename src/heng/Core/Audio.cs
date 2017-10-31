using System;
using System.Runtime.InteropServices;
using heng.Audio;

namespace heng
{
	internal static partial class Core
	{
		public static class Audio
		{
			[StructLayout(LayoutKind.Sequential)]
			public struct State
			{
				public readonly int DeviceID;
				
				public readonly AudioFormat Format;
				public readonly int Channels;
				public readonly int SampleRate;
				public readonly int SampleSize;
				
				public readonly int SamplesNeeded;
				
				public readonly Sounds.State Sounds;
				public readonly Mixer.State Mixer;
			};
			
			[DllImport(coreLib, EntryPoint = "Audio_GetSnapshot")]
			public static extern void GetSnapshot(out Audio.State state);
			
			[DllImport(coreLib, EntryPoint = "Audio_PushSound")]
			public static extern void PushSound();
			
			public static class Sounds
			{
				[StructLayout(LayoutKind.Sequential)]
				public struct State
				{
					public readonly int MaxSounds;
					public readonly int SoundCount;
				};
				
				[DllImport(coreLib, EntryPoint = "Audio_Sounds_LoadSound")]
				public static extern int LoadSound(string filePath);
				
				[DllImport(coreLib, EntryPoint = "Audio_Sounds_FreeSound")]
				public static extern void FreeSound(int soundID);
				
				[DllImport(coreLib, EntryPoint = "Audio_Sounds_CheckSound")]
				[return: MarshalAs(UnmanagedType.U1)]
				public static extern bool CheckSound(int soundID);
			};
			
			public static class Mixer
			{
				[StructLayout(LayoutKind.Sequential)]
				public struct State
				{
					public readonly Channels.State Channels;
					public readonly Mix.State Mix;
				};
				
				public static class Channels
				{
					public const int AUDIO_MIXER_CHANNELS_MAX = 64;

					[StructLayout(LayoutKind.Sequential)]
					public struct MixerChannelPanning
					{
						public readonly Byte Left;
						public readonly Byte Right;

						public MixerChannelPanning(byte left, byte right)
						{
							Left = left;
							Right = right;
						}
					};
					
					[StructLayout(LayoutKind.Sequential)]
					public struct MixerChannel
					{
						public readonly int SoundID;
						public readonly UInt32 DataPos;
						public readonly UInt32 DataLen;
						
						public readonly Byte Volume;
						public readonly MixerChannelPanning Panning;
					};
					
					[StructLayout(LayoutKind.Sequential)]
					public struct State
					{
						public readonly int ChannelCount;

						[MarshalAs(UnmanagedType.ByValArray, SizeConst = AUDIO_MIXER_CHANNELS_MAX)]
						public readonly MixerChannel[] Channels;
					};

					[DllImport(coreLib, EntryPoint = "Audio_Mixer_Channels_GetNextFreeChannel")]
					public static extern int GetNextFreeChannel();

					[DllImport(coreLib, EntryPoint = "Audio_Mixer_Channels_SetSound")]
					public static extern int SetSound(int channel, int soundID);
					
					[DllImport(coreLib, EntryPoint = "Audio_Mixer_Channels_SetVolume")]
					public static extern void SetVolume(int channel, Byte volume);
					
					[DllImport(coreLib, EntryPoint = "Audio_Mixer_Channels_SetPanning")]
					public static extern void SetPanning(int channel, MixerChannelPanning panning);
					
					[DllImport(coreLib, EntryPoint = "Audio_Mixer_Channels_CalcAttenuation")]
					public static extern void CalcAttenuation(int channel, Vector2 offsetFromListener, float maxDist);
					
					[DllImport(coreLib, EntryPoint = "Audio_Mixer_Channels_Advance")]
					public static extern void Advance(int channel, bool loop);
				};
				
				public static class Mix
				{
					[StructLayout(LayoutKind.Sequential)]
					public struct State
					{
						public readonly int AccumulatorSize;
						public readonly float StereoFalloffExponent;
					};
				};
			};
		};
	};
}