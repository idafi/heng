#include "log.h"

extern void Log_GetTimestamp(char *out);

intern log_level minLevel;

HEXPORT(void) Log_Console_Print(log_level level, char *msg)
{
	if(level >= minLevel)
	{
		if(msg)
		{
			char timestamp[13];
			Log_GetTimestamp(timestamp);

			switch(level)
			{
				case LOG_DEBUG:
					Log_Console_SetColor(CONSOLE_GREY, CONSOLE_BLACK);
					break;
				case LOG_NOTE:
					Log_Console_SetColor(CONSOLE_WHITE, CONSOLE_BLACK);
					break;
				case LOG_WARNING:
					Log_Console_SetColor(CONSOLE_YELLOW, CONSOLE_BLACK);
					break;
				case LOG_ERROR:
					Log_Console_SetColor(CONSOLE_RED, CONSOLE_BLACK);
					break;
				case LOG_FAILURE:
					Log_Console_SetColor(CONSOLE_YELLOW, CONSOLE_RED);
					break;
			}

			printf("%s: %s\n", timestamp, msg);
		}
		else
		{ LogError("couldn't print message to console: message is null"); }
	}
}

HEXPORT(void) Log_Console_SetMinLevel(log_level level)
{
	minLevel = level;
}