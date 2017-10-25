#pragma once

#include "_shared.h"

// - - - - - -
// main logger
// - - - - - -

typedef enum
{
	LOG_DEBUG,		// menial, fine-grain info for debugging
	LOG_NOTE,		// helpful notice that things are running ok
	LOG_WARNING,	// something weird happened, but nothing broke
	LOG_ERROR,		// something bad happened, and something broke
	LOG_FAILURE		// something so bad happened that the program needs to close
} log_level;

typedef struct
{
	log_level minLevelConsole;
	log_level minLevelFile;
} log_config;

#define LogDebug(msg, ...) Log_FormatToAll(LOG_DEBUG, msg, ##__VA_ARGS__)
#define LogNote(msg, ...) Log_FormatToAll(LOG_NOTE, msg, ##__VA_ARGS__)
#define LogWarning(msg, ...) Log_FormatToAll(LOG_WARNING, msg, ##__VA_ARGS__)
#define LogError(msg, ...) Log_FormatToAll(LOG_ERROR, msg, ##__VA_ARGS__)
#define LogFailure(msg, ...) Log_FormatToAll(LOG_FAILURE, msg, ##__VA_ARGS__)

bool Log_Init(log_config config);
void Log_Quit();

void Log_FormatToAll(log_level level, char *msg, ...);
void Log_PrintToAll(log_level level, char *msg);

// - - - - - -
// log console
// - - - - - -

typedef enum
{
	CONSOLE_BLACK,
	CONSOLE_GREY,
	CONSOLE_WHITE,
	CONSOLE_RED,
	CONSOLE_YELLOW
} console_color;

void Log_Console_SetColor(console_color foreground, console_color background);

HEXPORT(void) Log_Console_Print(log_level level, char *msg);
HEXPORT(void) Log_Console_SetMinLevel(log_level level);

// - - - - - -
// log file
// - - - - - -

HEXPORT(void) Log_File_Print(log_level level, char *msg);
HEXPORT(void) Log_File_SetMinLevel(log_level level);