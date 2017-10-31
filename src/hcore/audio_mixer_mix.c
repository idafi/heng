#include "audio.h"

extern SDL_AudioSpec Audio_GetSpec();
extern uint32 Audio_GetSampleSize();

intern uint32 accumulatorSize;
intern int64 *accumulator;

intern float falloffExp;

#define MixSamples(destType, dest, srcType, src, count) \
for(uint32 sMixI = 0; sMixI < (count); sMixI++) \
{ ((destType*)(dest))[sMixI] += (destType)(((srcType*)(src))[sMixI]); }

intern void MixSamplesIn(int64 *dest, void *src, SDL_AudioFormat srcFormat, uint32 sampleCount)
{
	AssertPtr(dest);
	AssertPtr(src);
	
	switch(srcFormat)
	{
		case AUDIO_U8:
			MixSamples(int64, dest, uint8, src, sampleCount);
			break;
		case AUDIO_S8:
			MixSamples(int64, dest, int8, src, sampleCount);
			break;
		case AUDIO_U16:
			MixSamples(int64, dest, uint16, src, sampleCount);
			break;
		case AUDIO_S16:
			MixSamples(int64, dest, int16, src, sampleCount);
			break;
		case AUDIO_S32:
			MixSamples(int64, dest, int32, src, sampleCount);
			break;
		case AUDIO_F32:
			MixSamples(int64, dest, float, src, sampleCount);
			break;
		default:
			LogError("can't mix samples: unknown format");
			break;
	}
}

intern void MixSamplesOut(void *dest, int64 *src, SDL_AudioFormat destFormat, uint32 sampleCount)
{
	AssertPtr(dest);
	AssertPtr(src);
	
	switch(destFormat)
	{
		case AUDIO_U8:
			MixSamples(uint8, dest, int64, src, sampleCount);
			break;
		case AUDIO_S8:
			MixSamples(int8, dest, int64, src, sampleCount);
			break;
		case AUDIO_U16:
			MixSamples(uint16, dest, int64, src, sampleCount);
			break;
		case AUDIO_S16:
			MixSamples(int16, dest, int64, src, sampleCount);
			break;
		case AUDIO_S32:
			MixSamples(int32, dest, int64, src, sampleCount);
			break;
		case AUDIO_F32:
			MixSamples(float, dest, int64, src, sampleCount);
			break;
		default:
			LogError("can't mix samples: unknown format");
			break;
	}
}

intern void AttenuateSamples(int64 *samples, uint32 sampleCount, uint8 volume, mixer_channel_panning panning)
{
	AssertPtr(samples);
	Assert((sampleCount % 2) == 0, "uneven number of samples");
	
	for(uint32 i = 0; i < sampleCount; i += 2)
	{	
		int64 *sL = &samples[i];
		int64 *sR = &samples[i + 1];
		
		float lScale = (float)(panning.left) / 255;
		float rScale = (float)(panning.right) / 255;
		float dScale = (float)(255 - volume) / 255;
		
		float lMod = (1 - rScale) + (float)(pow(dScale, falloffExp));
		float rMod = (1 - lScale) + (float)(pow(dScale, falloffExp));
		
		int64 lAdd = (int64)((float)(*sR) * lMod);
		int64 rAdd = (int64)((float)(*sL) * rMod);

		*sL += lAdd;
		*sR += rAdd;
		
		*sL = (int64)((float)(*sL) * lScale * (1 - dScale));
		*sR = (int64)((float)(*sR) * rScale * (1 - dScale));
	}
}

intern void NormalizeSamples(int64 *samples, uint32 sampleCount)
{
	AssertPtr(samples);
	
	// we'll use long doubles to be extra sure we don't lose any data
	long double scalar = 1;
	
	// we first need to check each sample to find the largest value
	for(uint32 s = 0; s < sampleCount; s++)
	{
		int64 *smp = &samples[s];
		
		// avoid divide by zero issues
		if(*smp != 0)
		{
			// get size of sample relative to the output format's limit
			long double sampleSize = (long double)(llabs(*smp));
			long double sizeLimit = SHRT_MAX;
			
			// adjust scalar if we've exceeded the limit more than a previous sample
			scalar = min(sizeLimit / sampleSize, scalar);
		}
	}
	
	// don't waste time normalizing if it isn't needed
	if(scalar < 1)
	{
		// just scale down each sample
		for(uint32 s = 0; s < sampleCount; s++)
		{ samples[s] = (int64)((long double)(samples[s]) * scalar); }
	}
}

bool Audio_Mixer_Mix_Init(float stereoFalloff)
{
	SDL_AudioSpec spec = Audio_GetSpec();
	accumulatorSize = spec.channels * spec.samples;
	
	falloffExp = stereoFalloff;
	
	accumulator = calloc(accumulatorSize, sizeof(int64));
	if(accumulator)
	{
		LogNote("audio mixer mix successfully initialized");
		return true;
	}
	else
	{ LogFailure("audio mixer mix failed to initialize: couldn't allocate accumulator"); }

	return false;
}

void Audio_Mixer_Mix_Quit()
{
	accumulatorSize = 0;
	
	free(accumulator);
	accumulator = NULL;
}

void Audio_Mixer_Mix_AddSamples(void *data, uint32 dataLen, uint8 volume, mixer_channel_panning panning)
{
	AssertPtr(data);
	AssertPtr(accumulator);
	
	SDL_AudioSpec spec = Audio_GetSpec();
	uint32 sampleSize = Audio_GetSampleSize();
	uint32 sampleCount = dataLen / sampleSize;
	
	int64 *buffer = calloc(sampleCount, sizeof(int64));
	MixSamplesIn(buffer, data, spec.format, sampleCount);
	
	if(spec.channels == 2)
	{ AttenuateSamples(buffer, sampleCount, volume, panning); }
	else
	{ LogWarning("couldn't attenuate samples: i haven't gotten around to anything but stereo yet"); }
	
	for(uint32 i = 0; i < sampleCount; i++)
	{ accumulator[i] += buffer[i]; }

	free(buffer);
}

void Audio_Mixer_Mix_GetMixedSamples(void *data, uint32 dataLen)
{
	AssertPtr(data);
	AssertPtr(accumulator);
	
	SDL_AudioSpec spec = Audio_GetSpec();
	uint32 sampleSize = Audio_GetSampleSize();
	uint32 sampleCount = dataLen / sampleSize;
	
	NormalizeSamples(accumulator, sampleCount);
	MixSamplesOut(data, accumulator, spec.format, sampleCount);
	
	memset(accumulator, 0, accumulatorSize * sizeof(int64));
}

struct audio_mixer_mix_state Audio_Mixer_Mix_GetSnapshot()
{
	struct audio_mixer_mix_state state = 
	{
		.accumulatorSize = accumulatorSize,
		.stereoFalloffExp = falloffExp
	};
	
	return state;
}