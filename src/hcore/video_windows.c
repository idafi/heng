#include "video.h"

typedef struct
{
	window_info info;
	bool isOpen;

	SDL_Window *window;
	SDL_Renderer *renderer;
} window;

intern window windows[VIDEO_WINDOWS_MAX];

intern void UpdateWindowInfo(int windowID)
{
	AssertWindow(windowID);

	int winX, winY, winW, winH;
	SDL_DisplayMode mode;
	SDL_Rect viewport;

	window *w = &windows[windowID];
	window_info *info = &w->info;

	int dIndex = SDL_GetWindowDisplayIndex(w->window);
	SDL_GetCurrentDisplayMode(dIndex, &mode);
	SDL_GetWindowPosition(w->window, &winX, &winY);
	SDL_GetWindowSize(w->window, &winW, &winH);
	SDL_RenderGetViewport(w->renderer, &viewport);

	char const * const title = SDL_GetWindowTitle(w->window);
	memcpy(info->title, SDL_GetWindowTitle(w->window), strlen(title));
	info->displayIndex = dIndex;
	info->refreshRate = mode.refresh_rate;

	info->displayRect = (screen_rect) { .w = mode.w, .h = mode.h };
	info->windowRect = (screen_rect) { winX, winY, winW, winH };
	info->viewportRect = (screen_rect) { viewport.x, viewport.y, viewport.w, viewport.h };
}

intern bool CreateWindow(int windowID, char *title, screen_rect rect, uint32 windowFlags, uint32 rendererFlags)
{
	AssertIndex(windowID, VIDEO_WINDOWS_MAX);
	AssertPtr(title);

	window *w = &windows[windowID];

	LogDebug("creating new window with\n\tID %i\n\ttitle '%s'\n\tposition (%i, %i)\n\tsize (%i, %i)",
			 windowID, title, rect.x, rect.y, rect.w, rect.h);

	w->window = SDL_CreateWindow(title, rect.x, rect.y, rect.w, rect.h, windowFlags);
	if(w->window)
	{
		w->renderer = SDL_CreateRenderer(w->window, -1, rendererFlags);
		if(w->renderer)
		{
			SDL_SetRenderDrawColor(w->renderer, 0xFF, 0xFF, 0xFF, 0xFF);

			w->info.id = windowID;
			w->isOpen = true;
			UpdateWindowInfo(windowID);

			LogNote("new window with ID %i successfully created", windowID);
			return true;
		}
		else
		{
			LogError("couldn't create SDL renderer for window %i\n\tSDL error: %s", windowID, SDL_GetError());
			SDL_DestroyWindow(w->window);
		}
	}
	else
	{ LogError("couldn't create SDL window for window %i\n\tSDL error: %s", windowID, SDL_GetError()); }

	return false;
}

intern void DestroyWindow(int windowID)
{
	AssertIndex(windowID, VIDEO_WINDOWS_MAX);

	window *w = &windows[windowID];
	SDL_DestroyRenderer(w->renderer);
	SDL_DestroyWindow(w->window);

	memset(&w->info, 0, sizeof(window_info));
	w->isOpen = false;
}

bool Video_Windows_Init()
{
	LogNote("video windows successfully initialized");
	return true;
}

void Video_Windows_Quit()
{
	for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
	{ DestroyWindow(i); }
}

HEXPORT(int) Video_Windows_OpenWindow(char *title, screen_rect rect, uint32 windowFlags, uint32 rendererFlags)
{
	if(title)
	{
		for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
		{
			if(!windows[i].isOpen)
			{
				CreateWindow(i, title, rect, windowFlags, rendererFlags);
				return i;
			}
		}

		LogError("couldn't create window: max number of windows (%i) already open", VIDEO_WINDOWS_MAX);
	}
	else
	{ LogError("couldn't create window: title is null"); }

	return -1;
}

HEXPORT(void) Video_Windows_CloseWindow(int windowID)
{
	if(windowID > -1 && windowID < VIDEO_WINDOWS_MAX)
	{
		if(windows[windowID].isOpen)
		{ DestroyWindow(windowID); }
		else
		{ LogWarning("couldn't close window %i: window is already closed", windowID); }
	}
	else
	{ LogWarning("couldn't close window %i: ID is invalid"); }
}

HEXPORT(bool) Video_Windows_CheckWindow(int windowID)
{
	if(windowID < 0 || windowID >= VIDEO_WINDOWS_MAX)
	{
		LogError("window ID '%i' is invalid", windowID);
		return false;
	}

	window *w = &windows[windowID];

	if(!w->isOpen)
	{
		LogError("window %i is closed", windowID);
		return false;
	}

	if(!w->window)
	{
		LogError("window %i has no SDL window", windowID);
		return false;
	}

	if(!w->renderer)
	{
		LogError("window %i has no SDL renderer", windowID);
		return false;
	}

	return true;
}

struct video_windows_state Video_Windows_GetSnapshot()
{
	struct video_windows_state state;
	for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
	{ state.windowInfo[i] = windows[i].info; }

	return state;
}