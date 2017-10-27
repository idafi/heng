#include "resource_map.h"

struct resource_map
{
	struct resource_map_value
	{
		void *data;
		int refCount;
	} *values;

	int valueCount;
	int maxCount;

	void *(*allocResource)(char *filePath);
	void (*freeResource)(void *resource);
};

// djb algorithm
HINLINE uint64 HashString(char *str)
{
	uint64 hash = 5381;
	int c;

	while((c = *(str++)))
	{
		hash = ((hash << 5) + hash) + c;
		// alternative: hash * 33 ^ c
	}

	return hash;
}

resource_map *ResourceMap_Create(int maxCount, void *(*allocResource)(char *filePath), void (*freeResource)(void *data))
{
	AssertSign(maxCount);
	AssertPtr(allocResource);
	AssertPtr(freeResource);

	resource_map *map = malloc(sizeof(resource_map));
	if(map)
	{
		map->values = calloc(maxCount, sizeof(struct resource_map_value));
		if(map->values)
		{
			map->maxCount = maxCount;
			map->valueCount = 0;

			map->allocResource = allocResource;
			map->freeResource = freeResource;
		}
		else
		{
			LogError("couldn't allocate memory for resource map value containers");
			free(map);
		}
	}
	else
	{ LogError("couldn't allocate memory for resource map"); }

	return map;
}

void ResourceMap_Free(resource_map *map)
{
	if(map)
	{
		if(map->valueCount > 0)
		{
			for(int i = 0; i < map->maxCount; i++)
			{
				struct resource_map_value *v = &map->values[i];
				if(v->data)
				{ map->freeResource(v->data); }
			}
		}

		free(map->values);
		free(map);

		map->values = NULL;
		map->maxCount = 0;
		map->allocResource = NULL;
		map->freeResource = NULL;
	}
	else
	{ LogWarning("couldn't free resource map: pointer is already NULL"); }
}

int ResourceMap_AllocResource(resource_map *map, char *filePath)
{
	AssertPtr(map);
	AssertPtr(filePath);

	if(map->valueCount < map->maxCount)
	{
		int i = (int)(HashString(filePath) % map->maxCount);
		struct resource_map_value *v = &map->values[i];

		// is data at this index already loaded?
		if(v->refCount > 0)
		{
			v->refCount++;
			return i;
		}

		// if not, try loading
		v->data = map->allocResource(filePath);
		if(v->data)
		{
			v->refCount = 1;
			map->valueCount++;
			return i;
		}
	}
	else
	{ LogError("couldn't allocate to resource map: no free value slots (limit: %i)", map->maxCount); }

	return -1;
}

void ResourceMap_FreeResource(resource_map *map, int index)
{
	AssertPtr(map);
	AssertIndex(index, map->maxCount);

	struct resource_map_value *v = &map->values[index];
	if(v->data)
	{
		v->refCount = max(v->refCount - 1, 0);
		if(v->refCount == 0)
		{
			map->freeResource(v->data);
			v->data = NULL;
			map->valueCount--;
		}
	}
	else
	{ LogWarning("couldn't free resource map value at index %i: pointer is already NULL"); }
}

void *ResourceMap_GetResource(resource_map *map, int index)
{
	Assert(ResourceMap_CheckResource(map, index), "resource map value at %i is invalid", index);

	return map->values[index].data;
}

bool ResourceMap_CheckResource(resource_map *map, int index)
{
	AssertPtr(map);

	if(index < 0 || index >= map->maxCount)
	{
		LogError("resource map index (%i) is invalid", index);
		return false;
	}

	if(!map->values[index].data)
	{
		LogError("resource map value %i has no data", index);
		return false;
	}

	if(map->values[index].refCount < 1)
	{
		LogError("resource map value %i has no references", index);
		return false;
	}

	return true;
}

int ResourceMap_GetResourceCount(resource_map *map)
{
	AssertPtr(map);

	return map->valueCount;
}

int ResourceMap_GetMaxCount(resource_map *map)
{
	AssertPtr(map);

	return map->maxCount;
}

int ResourceMap_GetRefCount(resource_map *map, int index)
{
	AssertPtr(map);
	Assert(ResourceMap_CheckResource(map, index), "resource map value at %i is invalid", index);

	return map->values[index].refCount;
}