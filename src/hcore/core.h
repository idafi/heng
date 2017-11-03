#pragma once

#include "_shared.h"
#include "audio.h"
#include "input.h"
#include "time.h"
#include "video.h"

// - - - - -
// events
// - - - - -

HEXPORT(void) Core_Events_Pump();
HEXPORT(bool) Core_Events_IsQuitRequested();

// - - - - - -
// event logging
// - - - - - -

typedef enum
{
	EVLOG_OFF			= 0x00,

	EVLOG_LOG_INPUT		= 0x01,
	EVLOG_LOG_DEVICE	= 0x02,
	EVLOG_LOG_WINDOW	= 0x04,
	EVLOG_LOG_QUIT		= 0x08,
	EVLOG_LOG_ALL		= 0x0F,

	EVLOG_SIMULATE		= 0x10
} event_log_mode;

// - - - - - -
// core
// - - - - - -

typedef struct
{
	log_config log;

	struct core_events_config
	{
		event_log_mode evLogMode;
	} events;

	audio_config audio;
} core_config;

HEXPORT(bool) Core_Init(core_config config);
HEXPORT(void) Core_Quit();