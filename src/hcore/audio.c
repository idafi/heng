#include "audio.h"

extern bool Audio_Sounds_Init(struct audio_sounds_config config);
extern void Audio_Sounds_Quit();
extern struct audio_sounds_state Audio_Sounds_GetSnapshot();

extern bool Audio_Mixer_Init(struct audio_mixer_config config);
extern void Audio_Mixer_Quit();
extern void Audio_Mixer_Mix_GetMixedSamples(void *data, uint32 dataLen);
extern struct audio_mixer_state Audio_Mixer_GetSnapshot();

intern SDL_AudioDeviceID device;
intern SDL_AudioSpec deviceSpec;

intern SDL_AudioFormat GetSDLFormat(audio_format format)
{
	switch(format)
	{
		case AUDIO_FORMAT_S8:
			return AUDIO_S8;
		case AUDIO_FORMAT_U8:
			return AUDIO_U8;
		case AUDIO_FORMAT_S16:
			return AUDIO_S16;
		case AUDIO_FORMAT_U16:
			return AUDIO_U16;
		case AUDIO_FORMAT_S32:
			return AUDIO_S32;
		case AUDIO_FORMAT_F32:
			return AUDIO_F32;
		case AUDIO_FORMAT_INVALID:
		default:
			LogError("invalid audio format: %i", format);
			return AUDIO_S16;
	}
}

intern audio_format GetHFormat(SDL_AudioFormat format)
{
	switch(format)
	{
		case AUDIO_S8:
			return AUDIO_FORMAT_S8;
		case AUDIO_U8:
			return AUDIO_FORMAT_U8;
		case AUDIO_S16:
			return AUDIO_FORMAT_S16;
		case AUDIO_U16:
			return AUDIO_FORMAT_U16;
		case AUDIO_S32:
			return AUDIO_FORMAT_S32;
		case AUDIO_F32:
			return AUDIO_FORMAT_F32;
		default:
			LogError("invalid audio format: %i", format);
			return AUDIO_FORMAT_INVALID;
	}
}

intern uint32 GetSampleSize(SDL_AudioFormat format)
{
	switch(format)
	{
		case AUDIO_S8:
		case AUDIO_U8:
			return 1;
		case AUDIO_S16:
		case AUDIO_U16:
			return 2;
		case AUDIO_S32:
		case AUDIO_F32:
			return 4;
		default:
			LogError("couldn't get sample size: invalid format");
			return 0;
	}
}

bool Audio_Init(audio_config config)
{
	if(SDL_InitSubSystem(SDL_INIT_AUDIO) >= 0)
	{
		LogNote("SDL audio successfully initialized");
		
		SDL_AudioSpec specWant =
		{
			.freq = config.sampleRate,
			.format = GetSDLFormat(config.format),
			.channels = config.channels,
			.samples = 4096
		};
		
		device = SDL_OpenAudioDevice(NULL, 0, &specWant, &deviceSpec, SDL_AUDIO_ALLOW_FORMAT_CHANGE);
		if(device > 0)
		{
			LogNote("audio device %i successfully opened", device);
			
			if(Audio_Sounds_Init(config.sounds) && Audio_Mixer_Init(config.mixer))
			{
				LogNote("audio successfully initialized");
				
				SDL_PauseAudioDevice(device, 0);
				return true;
			}
		}
		else
		{ LogError("failed to open audio device\n\tSDL error: %s", SDL_GetError()); }
	}
	else
	{ LogFailure("failed to initialize SDL audio\n\tSDL error: %s", SDL_GetError()); }

	return false;
}

void Audio_Quit()
{
	Audio_Mixer_Quit();
	Audio_Sounds_Quit();
	
	SDL_PauseAudioDevice(device, 1);
	SDL_CloseAudioDevice(device);
	SDL_QuitSubSystem(SDL_INIT_AUDIO);
}

SDL_AudioSpec Audio_GetSpec()
{
	return deviceSpec;
}

uint32 Audio_GetSampleSize()
{
	return GetSampleSize(deviceSpec.format);
}

uint32 Audio_GetBytesNeeded()
{
	uint32 queuedBytes = SDL_GetQueuedAudioSize(device);
	return deviceSpec.size - queuedBytes;
}

HEXPORT(void) Audio_PushSound()
{
	uint32 bytesNeeded = Audio_GetBytesNeeded();
	
	uint8 *data = calloc(bytesNeeded, sizeof(uint8));

	Audio_Mixer_Mix_GetMixedSamples(data, bytesNeeded);
	SDL_QueueAudio(device, data, bytesNeeded);

	free(data);
}

HEXPORT(void) Audio_GetSnapshot(audio_state *state)
{
	*state = (audio_state)
	{
		.deviceID = (int)(device),
		
		.format = GetHFormat(deviceSpec.format),
		.channels = deviceSpec.channels,
		.sampleRate = deviceSpec.freq,
		.sampleSize = Audio_GetSampleSize(),
		
		.sounds = Audio_Sounds_GetSnapshot(),
		.mixer = Audio_Mixer_GetSnapshot()
	};
}