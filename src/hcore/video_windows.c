#include "video.h"

extern void Video_Textures_DrawToRenderer(int windowID, int textureID, SDL_Renderer *renderer, screen_rect viewport, screen_point position, float rotation);

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
	w->info.id = -1;
	w->isOpen = false;
}

intern int GetWindowBySDLID(uint32 id)
{
	SDL_Window *sdlW = SDL_GetWindowFromID(id);
	if(sdlW)
	{
		for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
		{
			window *w = &windows[i];
			if(w->window == sdlW)
			{ return i; }
		}

		LogError("couldn't locate window using SDL ID: %i", id);
	}

	return -1;
}

bool Video_Windows_Init()
{
	for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
	{ windows[i].info.id = -1; }

	LogNote("video windows successfully initialized");
	return true;
}

void Video_Windows_Quit()
{
	for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
	{ DestroyWindow(i); }
}

void Video_Windows_Event(SDL_WindowEvent *ev)
{
	AssertPtr(ev);

	int wID = GetWindowBySDLID(ev->windowID);
	if(wID > -1)
	{
		switch(ev->event)
		{
			case SDL_WINDOWEVENT_CLOSE:
				Video_Windows_CloseWindow(wID);
				break;
			case SDL_WINDOWEVENT_MOVED:
			case SDL_WINDOWEVENT_SIZE_CHANGED:
			case SDL_WINDOWEVENT_RESIZED:
			case SDL_WINDOWEVENT_MAXIMIZED:
			case SDL_WINDOWEVENT_RESTORED:
				UpdateWindowInfo(wID);
				break;
		}
	}
}

HEXPORT(void) Video_Windows_OpenWindow(int windowID, char *title, screen_rect rect, uint32 windowFlags, uint32 rendererFlags)
{
	if(title)
	{
		if(!windows[windowID].isOpen)
		{ CreateWindow(windowID, title, rect, windowFlags, rendererFlags); }
		else
		{ LogError("couldn't create window %i: window is already open", windowID); }
	}
	else
	{ LogError("couldn't create window %i: title is null", windowID); }
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

HEXPORT(void) Video_Windows_SetWindowColor(int windowID, color color)
{
	if(Video_Windows_CheckWindow(windowID))
	{
		window *w = &windows[windowID];
		SDL_SetRenderDrawColor(w->renderer, color.r, color.g, color.b, color.a);
	}
	else
	{ LogError("couldn't set color for window %i", windowID); }
}

HEXPORT(void) Video_Windows_ClearWindow(int windowID)
{
	if(Video_Windows_CheckWindow(windowID))
	{
		window *w = &windows[windowID];
		SDL_RenderClear(w->renderer);
	}
	else
	{ LogError("couldn't clear window %i", windowID); }
}

HEXPORT(void) Video_Windows_PresentWindow(int windowID)
{
	if(Video_Windows_CheckWindow(windowID))
	{
		window *w = &windows[windowID];
		SDL_RenderPresent(w->renderer);
	}
	else
	{ LogError("couldn't present window %i", windowID); }
}

HEXPORT(void) Video_Windows_DrawPoint(int windowID, screen_point point)
{
	if(Video_Windows_CheckWindow(windowID))
	{
		window *w = &windows[windowID];

		// convert to sdl coordinates (y-down)
		point.y = w->info.viewportRect.h - point.y;
		SDL_RenderDrawPoint(w->renderer, point.x, point.y);
	}
	else
	{ LogError("couldn't draw point on window %i", windowID); }
}

HEXPORT(void) Video_Windows_DrawLine(int windowID, screen_line line)
{
	if(Video_Windows_CheckWindow(windowID))
	{
		window *w = &windows[windowID];
		int h = w->info.viewportRect.h;

		// convert to sdl coordinates (y-down)
		line.start.y = h - line.start.y;
		line.end.y = h - line.end.y;

		SDL_RenderDrawLine(w->renderer, line.start.x, line.start.y, line.end.x, line.end.y);
	}
	else
	{ LogError("couldn't draw line on window %i", windowID); }
}

HEXPORT(void) Video_Windows_DrawPoints(int windowID, points_draw_mode mode, screen_point *points, int count)
{
	if(Video_Windows_CheckWindow(windowID))
	{
		if(points)
		{
			if(count > 0)
			{
				window *w = &windows[windowID];
				int h = w->info.viewportRect.h;

				// convert to sdl coordinates (y-down)
				for(int i = 0; i < count; i++)
				{ points[i].y = h - points[i].y; }

				// HACK: this is probably less evil than other alternatives
				SDL_Point *sdlPoints = (SDL_Point *)(points);

				switch(mode)
				{
					case POINTS_DRAW_POINTS:
						SDL_RenderDrawPoints(w->renderer, sdlPoints, count);
						break;
					case POINTS_DRAW_LINES:
						SDL_RenderDrawLines(w->renderer, sdlPoints, count);
						break;
				}
			}
			else
			{ LogError("couldn't draw points on window %i: point count (%i) is less than 0", windowID, count); }
		}
		else
		{ LogError("couldn't draw points on window %i: points array is null", windowID); }
	}
	else
	{ LogError("couldn't draw points on window %i", windowID); }
}

HEXPORT(void) Video_Windows_DrawTexture(int windowID, int textureID, screen_point position, float rotation)
{
	if(Video_Windows_CheckWindow(windowID))
	{
		if(Video_Textures_CheckTexture(textureID))
		{
			window *w = &windows[windowID];
			Video_Textures_DrawToRenderer(windowID, textureID, w->renderer, w->info.viewportRect, position, rotation);
		}
		else
		{ LogError("couldn't draw texture %i on window %i", textureID, windowID); }
	}
	else
	{ LogError("couldn't draw texture %i on window %i", textureID, windowID); }
}

struct video_windows_state Video_Windows_GetSnapshot()
{
	struct video_windows_state state;
	for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
	{ state.windowInfo[i] = windows[i].info; }

	return state;
}