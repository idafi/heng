#include "video.h"

extern bool Video_Windows_Init();
extern void Video_Windows_Quit();
extern struct video_windows_state Video_Windows_GetSnapshot();

bool Video_Init()
{
	if(SDL_InitSubSystem(SDL_INIT_VIDEO) >= 0)
	{
		LogNote("SDL video successfully initialized");
		
		if(Video_Windows_Init())
		{
			LogNote("video successfully initialized");
			return true;
		}
	}
	else
	{ LogFailure("SDL video failed to initialized\n\tSDL error: %s", SDL_GetError()); }

	return false;
}

void Video_Quit()
{
	Video_Windows_Quit();
	SDL_QuitSubSystem(SDL_INIT_VIDEO);
}

HEXPORT(void) Video_GetSnapshot(video_state *state)
{
	*state = (video_state)
	{
		.windows = Video_Windows_GetSnapshot()
	};
}