#pragma once

#include "_shared.h"
#include "audio.h"
#include "input.h"
#include "video.h"

typedef struct
{
	log_config log;
	audio_config audio;
} core_config;

HEXPORT(bool) Core_Init(core_config config);
HEXPORT(void) Core_Quit();

// - - - - -
// events
// - - - - -

HEXPORT(void) Core_Events_Pump();
HEXPORT(bool) Core_Events_IsQuitRequested();