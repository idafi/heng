#pragma once

#include "_shared.h"

// - - - - - -
// Audio
// - - - - - -

typedef enum
{
	AUDIO_FORMAT_INVALID,
	AUDIO_FORMAT_U8,
	AUDIO_FORMAT_S8,
	AUDIO_FORMAT_U16,
	AUDIO_FORMAT_S16,
	AUDIO_FORMAT_S32,
	AUDIO_FORMAT_F32
} audio_format;

typedef struct
{
	audio_format format;
	int sampleRate;
	int channels;
	
	struct audio_sounds_config
	{
		int maxSounds;
	} sounds;
	
	struct audio_mixer_config
	{
		int channelCount;
		float attenuationThreshold;
		float stereoFalloffExponent;
	} mixer;
} audio_config;

bool Audio_Init(audio_config config);
void Audio_Quit();

HEXPORT(void) Audio_PushSound();

// - - - - - -
// sounds
// - - - - - -

typedef struct
{
	void *data;
	uint32 dataLen;
} sound;

#define AssertSound(id) Assert(Audio_Sounds_CheckSound(id), "sound with ID %i is invalid", id)

HEXPORT(int) Audio_Sounds_LoadSound(char *filePath);
HEXPORT(void) Audio_Sounds_FreeSound(int soundID);
HEXPORT(bool) Audio_Sounds_CheckSound(int soundID);

// - - - - - -
// mixer
// - - - - - -

typedef struct
{
	uint8 left;
	uint8 right;
	// more go here when 5/7.1 is a thing
} mixer_channel_panning;

#define AUDIO_MIXER_CHANNELS_MAX 64
typedef struct mixer_channel
{
	int soundID;
	uint32 dataPos;
	
	uint8 volume;
	mixer_channel_panning panning;
} mixer_channel;

HEXPORT(int) Audio_Mixer_Channels_GetNextFreeChannel();
HEXPORT(int) Audio_Mixer_Channels_SetSound(int channel, int soundID);
HEXPORT(void) Audio_Mixer_Channels_SetVolume(int channel, uint8 volume);
HEXPORT(void) Audio_Mixer_Channels_SetPanning(int channel, mixer_channel_panning panning);
HEXPORT(void) Audio_Mixer_Channels_CalcAttenuation(int channel, vector2 offsetFromListener, float maxDist);

HEXPORT(void) Audio_Mixer_Channels_Advance(int channel, bool loop);

// - - - - - -
// state snapshot
// - - - - - -

typedef struct
{
	int deviceID;
	
	audio_format format;
	int channels;
	int sampleRate;
	int sampleSize;
	
	int samplesNeeded;
	
	struct audio_sounds_state
	{
		int maxSounds;
		int soundCount;
	} sounds;
	
	struct audio_mixer_state
	{
		struct audio_mixer_channels_state
		{
			int channelCount;
			struct audio_mixer_channels_channel
			{
				int soundID;
				uint32 dataPos;
				uint32 dataLen;
				
				uint8 volume;
				mixer_channel_panning panning;
			} channels[AUDIO_MIXER_CHANNELS_MAX];
		} channels;
		
		struct audio_mixer_mix_state
		{
			int accumulatorSize;
			float stereoFalloffExp;
		} mix;
	} mixer;
} audio_state;

HEXPORT(void) Audio_GetSnapshot(audio_state *state);