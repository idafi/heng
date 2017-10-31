#include "audio.h"

extern bool Audio_Mixer_Channels_Init(int channelCt, float threshold);
extern void Audio_Mixer_Channels_Quit();
extern bool Audio_Mixer_Mix_Init(float stereoFalloff);
extern void Audio_Mixer_Mix_Quit();
extern struct audio_mixer_channels_state Audio_Mixer_Channels_GetSnapshot();
extern struct audio_mixer_mix_state Audio_Mixer_Mix_GetSnapshot();

bool Audio_Mixer_Init(struct audio_mixer_config config)
{
	if(Audio_Mixer_Channels_Init(config.channelCount, config.attenuationThreshold) &&
		Audio_Mixer_Mix_Init(config.stereoFalloffExponent))
	{
		LogNote("audio mixer successfully initialized");
		return true;
	}
	
	LogFailure("audio mixer failed to initialize");
	return false;
}

void Audio_Mixer_Quit()
{
	Audio_Mixer_Mix_Quit();
	Audio_Mixer_Channels_Quit();
}

struct audio_mixer_state Audio_Mixer_GetSnapshot()
{
	struct audio_mixer_state state = 
	{
		.channels = Audio_Mixer_Channels_GetSnapshot(),
		.mix = Audio_Mixer_Mix_GetSnapshot()
	};
	
	return state;
}