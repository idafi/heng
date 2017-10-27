#include "resource_map.h"
#include "video.h"

typedef struct
{
	int windowID;
	int surfaceID;
	SDL_Texture *tex;
} texture_instc;

#define MAX_SURFACES 1024
intern resource_map *surfaces;

#define TEX_CACHE_SIZE 256
intern int textureCacheUsage;
intern texture_instc textureCache[TEX_CACHE_SIZE];

intern void *AllocSurface(char *filePath)
{
	return SDL_LoadBMP(filePath);
}

intern void FreeSurface(void *surface)
{
	SDL_FreeSurface(surface);
}

intern void DestroyTextureInstc(int texInstcID)
{
	AssertIndex(texInstcID, TEX_CACHE_SIZE);

	texture_instc *t = &textureCache[texInstcID];
	t->windowID = -1;
	t->surfaceID = -1;

	SDL_DestroyTexture(t->tex);
	t->tex = NULL;
}

intern int GetTextureInstc(int windowID, int textureID, SDL_Renderer *renderer)
{
	AssertWindow(windowID);
	AssertTexture(textureID);
	AssertPtr(renderer);
	AssertPtr(surfaces);

	int nextFree = -1;

	// see if we have a texture ready for this context+surface
	for(int i = 0; i < textureCacheUsage; i++)
	{
		texture_instc *match = &textureCache[i];
		if(match->windowID == windowID && match->surfaceID == textureID)
		{
			// if so, scoot it to the top of the cache
			if(i > 0)
			{
				texture_instc *top = &textureCache[0];
				texture_instc buff = *match;

				*match = *top;
				*top = buff;
			}

			return 0;
		}

		// if no match, mark down the first free texture slot we find, in case we need it later
		if(nextFree < 0 && !match->tex)
		{ nextFree = i; }
	}

	// if there are no free slots, make one
	if(nextFree < 0)
	{
		DestroyTextureInstc(0);
		nextFree = 0;
	}

	// no texture was found, so set one up at the first free slot
	if(nextFree > -1)
	{
		texture_instc *t = &textureCache[nextFree];
		SDL_Surface *s = ResourceMap_GetResource(surfaces, textureID);

		if(s)
		{
			t->windowID = windowID;
			t->surfaceID = textureID;
			t->tex = SDL_CreateTextureFromSurface(renderer, s);
		}

		textureCacheUsage++;
		return nextFree;
	}

	LogError("couldn't create texture instance from texture ID %i on window ID %i", textureID, windowID);
	return -1;
}

bool Video_Textures_Init()
{
	surfaces = ResourceMap_Create(MAX_SURFACES, &AllocSurface, &FreeSurface);
	if(surfaces)
	{
		Video_Textures_ClearCache();

		LogNote("video textures successfully initialized");
		return true;
	}
	else
	{ LogFailure("failed to create surface map"); }

	LogFailure("failed to initialize video textures");
	return false;
}

void Video_Textures_Quit()
{
	Video_Textures_ClearCache();
	ResourceMap_Free(surfaces);
}

void Video_Textures_DrawToRenderer(int windowID, int textureID, SDL_Renderer *renderer, screen_rect viewport, screen_point position, float rotation)
{
	AssertTexture(textureID);
	AssertPtr(renderer);
	AssertPtr(surfaces);

	int texInstcID = GetTextureInstc(windowID, textureID, renderer);
	if(texInstcID > -1)
	{
		texture_instc *t = &textureCache[texInstcID];
		SDL_Surface *s = ResourceMap_GetResource(surfaces, t->surfaceID);

		AssertPtr(t->tex);

		int y = (viewport.h - position.y) - s->h;
		float ang = 360.0f - rotation;

		SDL_Rect r = { position.x, y, s->w, s->h };
		SDL_RenderCopyEx(renderer, t->tex, NULL, &r, ang, NULL, SDL_FLIP_NONE);
	}
	else
	{ LogError("couldn't draw texture with ID %i: couldn't get texture instance for window %i", textureID, windowID); }
}

HEXPORT(int) Video_Textures_LoadTexture(char *filePath)
{
	AssertPtr(surfaces);

	if(filePath)
	{ return ResourceMap_AllocResource(surfaces, filePath); }
	else
	{ LogError("couldn't load texture: file path is NULL"); }

	return -1;
}

HEXPORT(void) Video_Textures_FreeTexture(int textureID)
{
	AssertPtr(surfaces);

	ResourceMap_FreeResource(surfaces, textureID);
}

HEXPORT(bool) Video_Textures_CheckTexture(int textureID)
{
	AssertPtr(surfaces);

	if(textureID < 0 || textureID >= MAX_SURFACES)
	{
		LogError("texture ID is invalid: %i", textureID);
		return false;
	}

	if(!ResourceMap_CheckResource(surfaces, textureID))
	{
		LogError("no surface exists for texture ID: %i", textureID);
		return false;
	}

	return true;
}

HEXPORT(void) Video_Textures_ClearCache()
{
	for(int i = 0; i < TEX_CACHE_SIZE; i++)
	{ DestroyTextureInstc(i); }
}

struct video_textures_state Video_Textures_GetSnapshot()
{
	AssertPtr(surfaces);

	struct video_textures_state state =
	{
		.maxSurfaces = MAX_SURFACES,
		.surfaceCount = ResourceMap_GetResourceCount(surfaces),

		.cacheSize = TEX_CACHE_SIZE,
		.cacheUsage = textureCacheUsage
	};

	return state;
}