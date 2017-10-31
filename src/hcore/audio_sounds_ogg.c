#include <vorbis/vorbisfile.h>
#include "audio.h"

extern sound *Audio_Sounds_CreateSound(void *data, uint32 dataLen, SDL_AudioSpec spec);

sound *Audio_Sounds_OGG_Load(char *filePath)
{
	sound *s = NULL;

	OggVorbis_File file;
	if(ov_fopen(filePath, &file) >= 0)
	{
		uint32 sampleCount = (uint32)(ov_pcm_total(&file, -1));
		uint32 sampleRate = (uint32)(file.vi->rate);
		uint32 channels = (uint32)(file.vi->channels);
		
		// bytes = samples * channels * 2-byte sample size
		uint32 dataLen = sampleCount * channels * 2;
		uint8 *data = malloc(dataLen * sizeof(uint8));
		
		char buffer[4096];
		int bitstream = -1;
		
		uint32 bytesRead;
		uint32 bytePos = 0;
		
		do
		{
			bytesRead = ov_read(&file, buffer, 4096, 0, 2, 1, &bitstream);
			
			memcpy(data + bytePos, buffer, bytesRead);
			bytePos += bytesRead;
		}
		while(bytesRead > 0);

		ov_clear(&file);
		
		if(bytesRead == 0)
		{	
			SDL_AudioSpec soundSpec = 
			{
				.freq = sampleRate,
				.format = AUDIO_S16,
				.channels = channels
			};
	
			s = Audio_Sounds_CreateSound(data, dataLen, soundSpec);
		}
		else
		{ LogError("couldn't read ogg data"); }

		free(data);
	}
	else
	{ LogError("couldn't load ogg file"); }
	
	return s;
}