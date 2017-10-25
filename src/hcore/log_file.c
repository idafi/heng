#include "log.h"

extern void Log_GetTimestamp(char *out);

intern log_level minLevel;
intern FILE *logFile;

bool Log_File_Init()
{
	logFile = fopen("log.txt", "w");
	if(logFile)
	{
		LogDebug("successfully opened log file");
		return true;
	}

	LogFailure("failed to open log file");
	perror("file error");

	return false;
}

void Log_File_Quit()
{
	if(logFile)
	{
		fclose(logFile);
		logFile = NULL;
	}
	else
	{ LogWarning("tried to close log file, but log file was already closed"); }
}

HEXPORT(void) Log_File_Print(log_level level, char *msg)
{
	if(level >= minLevel)
	{
		if(msg)
		{
			if(logFile)
			{
				char timestamp[13];
				Log_GetTimestamp(timestamp);

				fprintf(logFile, "%s: %s\n", timestamp, msg);
			}
			else
			{ Log_Console_Print(LOG_ERROR, "can't print to log file: log file is closed"); }
		}
		else
		{ LogError("couldn't print message to file: message is null"); }
	}
}

HEXPORT(void) Log_File_SetMinLevel(log_level level)
{
	minLevel = level;
}