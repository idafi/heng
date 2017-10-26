#include "core.h"

intern bool quitRequested;

intern void HandleEvent(SDL_Event *ev)
{
	AssertPtr(ev);

	switch(ev->type)
	{
		case SDL_QUIT:
			quitRequested = true;
			break;
	}
}

bool Core_Events_Init()
{
	if(SDL_InitSubSystem(SDL_INIT_EVENTS) >= 0)
	{
		LogNote("SDL events successfully initialized");
		LogNote("core events successfully initialized");
		return true;
	}
	else
	{ LogFailure("failed to initialize core events\n\tSDL error: %s", SDL_GetError()); }

	return false;
}

void Core_Events_Quit()
{
	SDL_QuitSubSystem(SDL_INIT_EVENTS);
}

HEXPORT(void) Core_Events_Pump()
{
	SDL_Event ev;

	while(SDL_PollEvent(&ev))
	{ HandleEvent(&ev); }
}

HEXPORT(bool) Core_Events_IsQuitRequested()
{
	return quitRequested;
}