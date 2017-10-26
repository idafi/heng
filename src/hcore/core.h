#pragma once

#include "_shared.h"
#include "video.h"

typedef struct
{
	log_config log;
} core_config;

HEXPORT(bool) Core_Init(core_config config);
HEXPORT(void) Core_Quit();

HEXPORT(void) Core_Test();