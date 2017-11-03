#include "core.h"

intern FILE *evLog;
intern event_log_mode logMode;

#define LOG_BUFFER_SIZE 64
intern SDL_Event logBuffer[LOG_BUFFER_SIZE];
intern int logBufferHead, logBufferTail;

intern bool IsWriteMode()
{
	return (FlagTest(logMode, EVLOG_LOG_INPUT)
			|| FlagTest(logMode, EVLOG_LOG_WINDOW)
			|| FlagTest(logMode, EVLOG_LOG_DEVICE)
			|| FlagTest(logMode, EVLOG_LOG_QUIT));
}

intern char *GetLogFileMode()
{
	if(IsWriteMode())
	{
		if(FlagTest(logMode, EVLOG_SIMULATE))
		{ return "a+b"; }		// append mode if simulating before new writes
		else
		{ return "wb"; }		// otherwise, write-only
	}
	else if(FlagTest(logMode, EVLOG_SIMULATE))
	{ return "rb"; }			// simulating, but no writing, so read-only

	// whoops
	return "";
}

intern void PrintMode()
{
	char msg[150];

	strcpy(msg, "active event log modes:");

	if(FlagTest(logMode, EVLOG_LOG_INPUT))
	{ strcat(msg, "\n\tinput logging"); }
	if(FlagTest(logMode, EVLOG_LOG_DEVICE))
	{ strcat(msg, "\n\tdevice logging"); }
	if(FlagTest(logMode, EVLOG_LOG_WINDOW))
	{ strcat(msg, "\n\twindow logging"); }
	if(FlagTest(logMode, EVLOG_LOG_QUIT))
	{ strcat(msg, "\n\tquit logging"); }
	if(FlagTest(logMode, EVLOG_SIMULATE))
	{ strcat(msg, "\n\tsimulation"); }
	
	LogDebug(msg);
}

intern void ClearLogBuffer()
{
	memset(logBuffer, 0, sizeof(SDL_Event) * LOG_BUFFER_SIZE);
	logBufferHead = 0;
	logBufferTail = 0;
}

intern void WriteLogBuffer()
{
	if(IsWriteMode())
	{
		AssertPtr(evLog);

		fwrite(logBuffer, sizeof(SDL_Event), logBufferHead, evLog);
		ClearLogBuffer();
	}
}

intern bool ReadToLogBuffer()
{
	AssertPtr(evLog);

	if(!feof(evLog))
	{
		logBufferHead = (int)(fread(logBuffer, sizeof(SDL_Event), LOG_BUFFER_SIZE, evLog));
		logBufferTail = 0;

		return true;
	}

	return false;
}

intern void WriteEvent(SDL_Event *ev)
{
	AssertPtr(ev);

	if(IsWriteMode())
	{
		logBuffer[logBufferHead++] = *ev;

		if(logBufferHead >= LOG_BUFFER_SIZE)
		{ WriteLogBuffer(); }
	}
}

bool Core_Events_Log_Init(event_log_mode mode)
{
	logMode = mode;

	if(logMode != EVLOG_OFF)
	{
		char *mode = GetLogFileMode();
		evLog = fopen("evLog.dat", mode);

		if(!evLog)
		{
			LogFailure("couldn't open event log with mode: %s", mode);
			return false;
		}

		// log is open. be safe and rewind
		rewind(evLog);

		LogNote("core events log successfully initialized");
		LogDebug("event log filemode: %s", mode);
		PrintMode();
	}

	return true;
}

void Core_Events_Log_Quit()
{
	if(evLog)
	{
		WriteLogBuffer();

		fclose(evLog);
		evLog = NULL;
	}
}

void Core_Events_Log_LogEvent(SDL_Event *ev)
{
	if(logMode != EVLOG_OFF)
	{
		AssertPtr(ev);

		switch(ev->type)
		{
			case SDL_KEYDOWN:
			case SDL_KEYUP:
			case SDL_MOUSEBUTTONDOWN:
			case SDL_MOUSEBUTTONUP:
			case SDL_CONTROLLERBUTTONDOWN:
			case SDL_CONTROLLERBUTTONUP:
			case SDL_CONTROLLERAXISMOTION:
				if(FlagTest(logMode, EVLOG_LOG_INPUT))
				{ WriteEvent(ev); }
				break;
			case SDL_JOYDEVICEADDED:
			case SDL_JOYDEVICEREMOVED:
				if(FlagTest(logMode, EVLOG_LOG_DEVICE))
				{ WriteEvent(ev); }
			case SDL_WINDOWEVENT:
				if(FlagTest(logMode, EVLOG_LOG_WINDOW))
				{ WriteEvent(ev); }
				break;
			case SDL_QUIT:
				if(FlagTest(logMode, EVLOG_LOG_QUIT))
				{ WriteEvent(ev); }
				break;
		}
	}
}

bool Core_Events_Log_PollEvent(SDL_Event *ev, uint32 time)
{
	// don't bother if not in simulate mode
	if(FlagTest(logMode, EVLOG_SIMULATE))
	{
		AssertPtr(ev);

		// look for pending events, attempting buffer refill if we reached the end
		if(logBufferTail < logBufferHead || ReadToLogBuffer())
		{
			SDL_Event *next = &logBuffer[logBufferTail];

			// if event timestamp has arrived (or passed), fire it off
			if(next->common.timestamp <= time)
			{
				logBufferTail++;

				*ev = *next;
				return true;
			}
		}
	}

	return false;
}