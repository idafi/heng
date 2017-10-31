#include "audio.h"

extern SDL_AudioSpec Audio_GetSpec();
extern sound *Audio_Sounds_CreateSound(void *data, uint32 dataLen, SDL_AudioSpec spec);

sound *Audio_Sounds_WAV_Load(char *filePath)
{
	AssertPtr(filePath);
	
	uint8 *data;
	uint32 dataLen;
	SDL_AudioSpec wavSpec = Audio_GetSpec();
	
	if(SDL_LoadWAV(filePath, &wavSpec, &data, &dataLen))
	{
		LogDebug("successfully loaded data from wav file '%s'", filePath);
		
		sound *s = Audio_Sounds_CreateSound(data, dataLen, wavSpec);
		SDL_FreeWAV(data);
		
		return s;
	}
	else
	{ LogError("couldn't load WAV file at '%s'\n\tSDL error: %s", filePath, SDL_GetError()); }

	return NULL;
}