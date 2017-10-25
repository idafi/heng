#include "core.h"

HEXPORT(bool) Core_Init(core_config config)
{
	if(SDL_Init(SDL_INIT_EVENTS) >= 0)
	{
		if(Log_Init(config.log))
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
	Log_Quit();
	SDL_Quit();
}

HEXPORT(void) Core_Test()
{
	LogDebug("core: psst");
	LogNote("core: hello");
	LogWarning("core: uh");
	LogError("core: oh no");
	LogFailure("core: whoops");
}