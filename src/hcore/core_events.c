#include "core.h"

extern bool Core_Events_Log_Init(event_log_mode mode);
extern void Core_Events_Log_Quit();
extern void Core_Events_Log_LogEvent(SDL_Event *ev);
extern bool Core_Events_Log_PollEvent(SDL_Event *ev, uint32 time);

intern bool quitRequested;

intern void HandleEvent(SDL_Event *ev)
{
	AssertPtr(ev);

	switch(ev->type)
	{
		case SDL_KEYDOWN:
		case SDL_KEYUP:
			Input_KeyEvent(&ev->key);
			break;
		case SDL_MOUSEBUTTONDOWN:
		case SDL_MOUSEBUTTONUP:
			Input_MButtonEvent(&ev->button);
			break;
		case SDL_CONTROLLERBUTTONDOWN:
		case SDL_CONTROLLERBUTTONUP:
			Input_CButtonEvent(&ev->cbutton);
			break;
		case SDL_CONTROLLERAXISMOTION:
			Input_AxisEvent(&ev->caxis);
			break;
		case SDL_JOYDEVICEADDED:
			Input_JoystickOpenEvent(&ev->jdevice);
			break;
		case SDL_JOYDEVICEREMOVED:
			Input_JoystickCloseEvent(&ev->jdevice);
			break;
		case SDL_WINDOWEVENT:
			Video_Windows_Event(&ev->window);
			break;
		case SDL_QUIT:
			quitRequested = true;
			break;
	}
}

bool Core_Events_Init(struct core_events_config config)
{
	if(SDL_InitSubSystem(SDL_INIT_EVENTS) >= 0)
	{
		LogNote("SDL events successfully initialized");

		if(Core_Events_Log_Init(config.evLogMode))
		{
			LogNote("core events successfully initialized");
			return true;
		}
	}
	else
	{ LogFailure("failed to initialize core events\n\tSDL error: %s", SDL_GetError()); }

	return false;
}

void Core_Events_Quit()
{
	Core_Events_Log_Quit();
	SDL_QuitSubSystem(SDL_INIT_EVENTS);
}

HEXPORT(void) Core_Events_Pump()
{
	SDL_Event ev;

	while(Core_Events_Log_PollEvent(&ev, Time_GetTicks()))
	{ SDL_PushEvent(&ev); }

	while(SDL_PollEvent(&ev))
	{
		Core_Events_Log_LogEvent(&ev);
		HandleEvent(&ev);
	}
}

HEXPORT(bool) Core_Events_IsQuitRequested()
{
	return quitRequested;
}