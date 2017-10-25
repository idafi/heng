#include <stdarg.h>
#include <time.h>
#include "log.h"

extern bool Log_File_Init();
extern void Log_File_Quit();

#define MAX_MSG_LENGTH 512

intern time_t initTime;

bool Log_Init(log_config config)
{
	initTime = time(NULL);

	Log_Console_SetMinLevel(config.minLevelConsole);
	Log_File_SetMinLevel(config.minLevelFile);

	if(Log_File_Init())
	{
		LogNote("log successfully initialized");
		return true;
	}

	LogFailure("log failed to initialize");
	return false;
}

void Log_Quit()
{
	Log_File_Quit();
}

void Log_FormatToAll(log_level level, char *msg, ...)
{
	AssertPtr(msg);

	char out[MAX_MSG_LENGTH];

	va_list args;
	va_start(args, msg);

	vsnprintf(out, MAX_MSG_LENGTH, msg, args);
	Log_PrintToAll(level, msg);

	va_end(args);
}

void Log_PrintToAll(log_level level, char *msg)
{
	AssertPtr(msg);

	Log_Console_Print(level, msg);
	Log_File_Print(level, msg);
}

void Log_GetTimestamp(char *out)
{
	AssertPtr(out);

	uint32 currentMS = SDL_GetTicks();					// ms since init
	time_t currentTime = initTime + (currentMS / 1000);	// seconds at init + seconds since init
	struct tm *local = localtime(&currentTime);			// convert to local time
	uint32 msRem = currentMS % 1000;					// re-append ms lost from seconds conversion

	char base[9];
	strftime(base, 9, "%T", local);

	snprintf(out, 13, "%s.%03u", base, msRem);
}