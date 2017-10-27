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
	int id;
	char title[VIDEO_WINDOWS_TITLE_MAX];

	int displayIndex;
	int refreshRate;

	screen_rect displayRect;
	screen_rect windowRect;
	screen_rect viewportRect;
} window_info;

typedef enum
{
	POINTS_DRAW_POINTS,
	POINTS_DRAW_LINES
} points_draw_mode;

HEXPORT(int) Video_Windows_OpenWindow(char *title, screen_rect rect, uint32 windowFlags, uint32 rendererFlags);
HEXPORT(void) Video_Windows_CloseWindow(int windowID);
HEXPORT(bool) Video_Windows_CheckWindow(int windowID);

HEXPORT(void) Video_Windows_SetWindowColor(int windowID, color color);
HEXPORT(void) Video_Windows_ClearWindow(int windowID);
HEXPORT(void) Video_Windows_PresentWindow(int windowID);

HEXPORT(void) Video_Windows_DrawPoint(int windowID, screen_point point);
HEXPORT(void) Video_Windows_DrawLine(int windowID, screen_line line);
HEXPORT(void) Video_Windows_DrawPoints(int windowID, points_draw_mode mode, screen_point *points, int count);
HEXPORT(void) Video_Windows_DrawTexture(int windowID, int textureID, screen_point position, float rotation);

// - - - - - -
// textures
// - - - - - -

#define AssertTexture(id) Assert(Video_Textures_CheckTexture(id), "texture %i is invalid", id)

HEXPORT(int) Video_Textures_LoadTexture(char *filePath);
HEXPORT(void) Video_Textures_FreeTexture(int textureID);
HEXPORT(bool) Video_Textures_CheckTexture(int textureID);

HEXPORT(void) Video_Textures_ClearCache();

// - - - - - -
// state snapshot
// - - - - - -

typedef struct
{
	struct video_windows_state
	{
		window_info windowInfo[VIDEO_WINDOWS_MAX];
	} windows;

	struct video_textures_state
	{
		int maxSurfaces;
		int surfaceCount;

		int cacheSize;
		int cacheUsage;
	} textures;
} video_state;

HEXPORT(void) Video_GetSnapshot(video_state *state);