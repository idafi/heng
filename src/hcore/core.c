#include "core.h"

extern bool Core_Events_Init();
extern void Core_Events_Quit();

HEXPORT(bool) Core_Init(core_config config)
{
	if(SDL_Init(0) >= 0)
	{
		if(Log_Init(config.log) && Core_Events_Init() && Video_Init() && Audio_Init(config.audio))
		{
			LogNote("core successfully initialized");
			return true;
		}
	}
	else
	{ LogFailure("failed to initialize SDL\n\tSDL error: %s"); }

	return false;
}

HEXPORT(void) Core_Quit()
{
	Audio_Quit();
	Video_Quit();
	Core_Events_Quit();
	Log_Quit();

	SDL_Quit();
}