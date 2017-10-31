#include "audio.h"
#include "resource_map.h"

extern SDL_AudioSpec Audio_GetSpec();
extern sound *Audio_Sounds_WAV_Load(char *filePath);
extern sound *Audio_Sounds_OGG_Load(char *filePath);

intern int maxSounds;
intern resource_map *sounds;

intern void *AllocSound(char *filePath)
{
	AssertPtr(filePath);
	
	LogDebug("loading sound from '%s'", filePath);
	
	char *ext = strrchr(filePath, '.');
	if(ext)
	{
		ext++;
		
		LogDebug("sound has extension '%s'", ext);
		
		if(strcmp(ext, "wav") == 0)
		{ return Audio_Sounds_WAV_Load(filePath); }
		else if(strcmp(ext, "ogg") == 0)
		{ return Audio_Sounds_OGG_Load(filePath); }
	
		LogError("couldn't load sound file at '%s': unrecognized extension ('%s')", filePath, ext);
	}
	else
	{ LogError("couldn't load sound file at '%s': file has no extension", filePath); }

	return NULL;
}

intern void FreeSound(void *data)
{
	if(data)
	{
		sound *s = (sound*)(data);
		free(s->data);
		free(s);
	}
	else
	{ LogWarning("couldn't free sound: data pointer is already NULL"); }
}

intern bool ConvertSound(sound *s, SDL_AudioSpec soundSpec, SDL_AudioSpec desiredSpec)
{
	AssertPtr(s);
	
	bool cvtResult;

	SDL_AudioCVT cvt;
	SDL_BuildAudioCVT(&cvt,
		soundSpec.format, soundSpec.channels, soundSpec.freq,
		desiredSpec.format, desiredSpec.channels, desiredSpec.freq);
	
	if(cvt.needed)
	{
		LogDebug("converting sound");
		
		size_t bufferSize = (s->dataLen * cvt.len_mult);
		uint8 *buffer = malloc(bufferSize * sizeof(uint8));

		memcpy(buffer, s->data, s->dataLen);
		
		cvt.buf = buffer;
		cvt.len = s->dataLen;

		cvtResult = (SDL_ConvertAudio(&cvt) >= 0);

		if(cvtResult)
		{
			size_t convertedSize = (size_t)(s->dataLen * cvt.len_ratio);
			s->data = realloc(s->data, convertedSize);
			s->dataLen = (uint32)(convertedSize);
			memcpy(s->data, buffer, convertedSize);
			
			LogDebug("successfully converted sound");
		}
		else
		{ LogError("couldn't convert sound\n\tSDL error: %s", SDL_GetError()); }

		free(buffer);
	}

	return (!cvt.needed || cvtResult);
}

bool Audio_Sounds_Init(struct audio_sounds_config config)
{
	AssertSign(config.maxSounds);
	
	sounds = ResourceMap_Create(config.maxSounds, &AllocSound, &FreeSound);
	if(sounds)
	{
		maxSounds = config.maxSounds;
		return true;
	}
	
	return false;
}

void Audio_Sounds_Quit()
{
	ResourceMap_Free(sounds);
	sounds = NULL;
	
	maxSounds = 0;
}

sound *Audio_Sounds_CreateSound(void *data, uint32 dataLen, SDL_AudioSpec spec)
{
	AssertPtr(data);
	
	sound *s = malloc(sizeof(sound));
	if(s)
	{
		s->data = malloc(dataLen);
		if(s->data)
		{
			s->dataLen = dataLen;
			memcpy(s->data, data, dataLen);
			
			if(!ConvertSound(s, spec, Audio_GetSpec()))
			{
				LogError("couldn't create sound: failed to convert to desired format");
				
				free(s->data);
				free(s);
			}
		}
		else
		{ LogError("couldn't create sound: failed to allocate memory for sound samples"); }
	}
	else
	{ LogError("couldn't create sound: failed to allocate memory for sound structure"); }
	
	LogDebug("successfully created sound");
	return s;
}

sound *Audio_Sounds_GetSound(int soundID)
{
	AssertSound(soundID);
	AssertPtr(sounds);
	
	return ResourceMap_GetResource(sounds, soundID);
}

HEXPORT(int) Audio_Sounds_LoadSound(char *filePath)
{
	AssertPtr(filePath);
	AssertPtr(sounds);
	
	return ResourceMap_AllocResource(sounds, filePath);
}

HEXPORT(void) Audio_Sounds_FreeSound(int soundID)
{
	AssertPtr(sounds);
	
	ResourceMap_FreeResource(sounds, soundID);
}

HEXPORT(bool) Audio_Sounds_CheckSound(int soundID)
{
	AssertPtr(sounds);
	
	if(soundID < 0 || soundID >= maxSounds)
	{
		LogError("sound ID is invalid: %i", soundID);
		return false;
	}
	
	if(!ResourceMap_CheckResource(sounds, soundID))
	{
		LogError("no sound exists for ID: %i", soundID);
		return false;
	}
	
	return true;
}

struct audio_sounds_state Audio_Sounds_GetSnapshot()
{
	AssertPtr(sounds);
	
	struct audio_sounds_state state =
	{
		.maxSounds = maxSounds,
		.soundCount = ResourceMap_GetResourceCount(sounds)
	};
	
	return state;
}