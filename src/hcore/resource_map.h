#pragma once

#include "_shared.h"

typedef struct resource_map resource_map;

resource_map *ResourceMap_Create(int maxCount, void *(*allocResource)(char *filePath), void(*freeResource)(void *data));
void ResourceMap_Free(resource_map *map);

int ResourceMap_AllocResource(resource_map *map, char *filePath);
void ResourceMap_FreeResource(resource_map *map, int index);
void *ResourceMap_GetResource(resource_map *map, int index);
bool ResourceMap_CheckResource(resource_map *map, int index);

int ResourceMap_GetResourceCount(resource_map *map);
int ResourceMap_GetMaxCount(resource_map *map);
int ResourceMap_GetRefCount(resource_map *map, int index);