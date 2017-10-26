#pragma once

#include "_shared.h"

// - - - - - -
// shared structures
// - - - - - -

typedef struct
{
	int x, y;
} screen_point;

typedef struct
{
	int x, y;
	int w, h;
} screen_rect;

typedef struct
{
	screen_point start, end;
} screen_line;

typedef struct
{
	byte r, g, b, a;
} color;

#define COLOR_CLEAR ((color) { 0x00, 0x00, 0x00, 0x00 })
#define COLOR_BLACK ((color) { 0x00, 0x00, 0x00, 0xFF })
#define COLOR_RED ((color) { 0xFF, 0x00, 0x00, 0xFF })
#define COLOR_GREEN ((color) { 0x00, 0xFF, 0x00, 0xFF })
#define COLOR_BLUE ((color) { 0x00, 0x00, 0xFF, 0xFF })
#define COLOR_YELLOW ((color) { 0xFF, 0xFF, 0x00, 0xFF })
#define COLOR_MAGENTA ((color) { 0xFF, 0x00, 0xFF, 0xFF })
#define COLOR_CYAN ((color) { 0x00, 0xFF, 0xFF, 0xFF })
#define COLOR_WHITE ((color) { 0xFF, 0xFF, 0xFF, 0xFF })

// - - - - - -
// video
// - - - - - -

bool Video_Init();
void Video_Quit();

// - - - - - -
// windows
// - - - - - -

#define VIDEO_WINDOWS_TITLE_MAX 128
#define VIDEO_WINDOWS_MAX 4

#define AssertWindow(id) Assert(Video_Windows_CheckWindow(id), "window %i is invalid", id)

typedef struct
{
	char title[VIDEO_WINDOWS_TITLE_MAX];
	screen_rect windowRect;
} window_config;

typedef struct
{
	int id;
	char title[VIDEO_WINDOWS_TITLE_MAX];

	int displayIndex;
	int refreshRate;

	screen_rect displayRect;
	screen_rect windowRect;
	screen_rect viewportRect;
} window_info;

HEXPORT(int) Video_Windows_OpenWindow(char *title, screen_rect rect, uint32 windowFlags, uint32 rendererFlags);
HEXPORT(void) Video_Windows_CloseWindow(int windowID);
HEXPORT(bool) Video_Windows_CheckWindow(int windowID);

// - - - - - -
// state snapshot
// - - - - - -

typedef struct
{
	struct video_windows_state
	{
		window_info windowInfo[VIDEO_WINDOWS_MAX];
	} windows;
} video_state;

HEXPORT(void) Video_GetSnapshot(video_state *state);