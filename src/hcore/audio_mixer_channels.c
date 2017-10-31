#include "audio.h"

extern uint32 Audio_GetBytesNeeded();
extern sound *Audio_Sounds_GetSound(int soundID);
extern void Audio_Mixer_Mix_AddSamples(void *data, uint32 dataLen, uint8 volume, mixer_channel_panning panning);

intern int channelCount;
intern mixer_channel channels[AUDIO_MIXER_CHANNELS_MAX];

intern int attenuationThreshold;

bool Audio_Mixer_Channels_Init(int channelCt, int threshold)
{
	AssertCount(channelCt, AUDIO_MIXER_CHANNELS_MAX);
	
	channelCount = channelCt;
	attenuationThreshold = threshold;
	
	for(int i = 0; i < channelCount; i++)
	{ channels[i].soundID = -1; }
	
	LogNote("audio mixer channels successfully initialized");
	return true;
}

void Audio_Mixer_Channels_Quit()
{
	channelCount = 0;
}

HEXPORT(int) Audio_Mixer_Channels_GetNextFreeChannel()
{
	for(int i = 0; i < channelCount; i++)
	{
		if(channels[i].soundID < 0)
		{ return i; }
	}

	return -1;
}

HEXPORT(int) Audio_Mixer_Channels_SetSound(int channel, int soundID)
{
	AssertSign(channel);

	if(channel < channelCount)
	{		
		if(soundID < 0 || Audio_Sounds_CheckSound(soundID))
		{
			channels[channel].soundID = soundID;
			channels[channel].dataPos = 0;
				
			return channel;
		}
		else
		{ LogError("can't assign sound: sound with ID %i is invalid", soundID); }
	}
	else
	{ LogError("can't assign sound: channel index %i exceeds channel count (%i)", channel, channelCount); }
	
	return -1;
}

HEXPORT(void) Audio_Mixer_Channels_SetVolume(int channel, uint8 volume)
{
	if(channel > -1 && channel < channelCount)
	{ channels[channel].volume = volume; }
	else
	{ LogError("can't set volume for channel %i: channel index is invalid (max: %i)", channel, channelCount); }
}

HEXPORT(void) Audio_Mixer_Channels_SetPanning(int channel, mixer_channel_panning panning)
{
	if(channel > -1 && channel < channelCount)
	{ channels[channel].panning = panning; }
	else
	{ LogError("can't set panning for channel %i: channel index is invalid (max: %i)", channel, channelCount); }
}

HEXPORT(void) Audio_Mixer_Channels_CalcAttenuation(int channel, vector2 offsetFromListener, float maxDist)
{
	if(channel > -1 && channel < channelCount)
	{
		float sqrMag = VectorSqrMagnitude(offsetFromListener);
		float dist = min(sqrMag / (maxDist * maxDist), 1);
		
		float dot = VectorDot(VectorNormalize(offsetFromListener), VECTOR2_RIGHT);
		float lScale = (float)(fabs(max(dot - 1, -1)));
		float rScale = min(dot + 1, 1);
		
		uint8 volume = 255 - (uint8)(255 * dist);
		uint8 left = (uint8)(255 * lScale);
		uint8 right = (uint8)(255 * rScale);
		
		Audio_Mixer_Channels_SetVolume(channel, volume);
		Audio_Mixer_Channels_SetPanning(channel, (mixer_channel_panning) { .left = left, .right = right });
	}
	else
	{ LogError("can't calculate attenuation for channel %i: channel index is invalid (max: %i)", channel, channelCount); }
}

HEXPORT(void) Audio_Mixer_Channels_Advance(int channel, bool loop)
{
	if(channel > -1 && channel < channelCount)
	{
		mixer_channel *ch = &channels[channel];
		if(ch->soundID > -1)
		{
			if(Audio_Sounds_CheckSound(ch->soundID))
			{
				sound *s = Audio_Sounds_GetSound(ch->soundID);
				if(loop || ch->dataPos < s->dataLen)
				{	
					uint32 bytesUsed;
					uint32 bytesNeeded = Audio_GetBytesNeeded();
					uint32 bytesAvailable = min(s->dataLen - ch->dataPos, bytesNeeded);
					
					uint8 *chBuffer = malloc(bytesNeeded * sizeof(uint8));
					memcpy(chBuffer, (uint8 *)(s->data) + ch->dataPos, bytesAvailable);
					
					if(loop && bytesAvailable < bytesNeeded)
					{
						int loopBytes = bytesNeeded - bytesAvailable;
						memcpy(chBuffer + bytesAvailable, s->data, loopBytes);
						
						ch->dataPos = loopBytes;
						bytesUsed = bytesNeeded;
					}
					else
					{
						memset(chBuffer + bytesAvailable, 0, bytesNeeded - bytesAvailable);
						
						ch->dataPos += bytesAvailable;
						bytesUsed = bytesAvailable;
					}
					
					Audio_Mixer_Mix_AddSamples(chBuffer, bytesUsed, ch->volume, ch->panning);
					free(chBuffer);
				}
				else
				{ LogWarning("couldn't advance channel %i: channel has finished playing its sound (ID: %i)", channel, ch->soundID); }
			}
			else
			{ LogError("couldn't advance channel %i: channel's sound (ID: %i) is invalid", ch->soundID); }
		}
		else
		{ LogWarning("couldn't advance channel %i: channel has no sound ID assigned"); }
	}
	else
	{ LogError("couldn't advance channel %i: channel index is invalid (max: %i)", channel, channelCount); }
}

struct audio_mixer_channels_state Audio_Mixer_Channels_GetSnapshot()
{
	struct audio_mixer_channels_state state;
	state.channelCount = channelCount;
	
	for(int i = 0; i < channelCount; i++)
	{
		mixer_channel *ch = &channels[i];
		state.channels[i] = (struct audio_mixer_channels_channel)
		{
			.soundID = ch->soundID,
			.dataPos = ch->dataPos,
			.volume = ch->volume,
			.panning = ch->panning
		};
		
		if(ch->soundID > -1 && Audio_Sounds_CheckSound(ch->soundID))
		{
			sound *s = Audio_Sounds_GetSound(ch->soundID);
			state.channels[i].dataLen = s->dataLen;
		}
	}
	
	return state;
}