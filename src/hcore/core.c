#include "core.h"

HEXPORT(bool) Core_Init(core_config config)
{
	if(SDL_Init(SDL_INIT_EVENTS) >= 0)
	{
		if(Log_Init(config.log) && Video_Init())
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
	Video_Quit();
	Log_Quit();

	SDL_Quit();
}