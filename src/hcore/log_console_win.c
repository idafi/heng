#include "log.h"

#if defined(WIN_X64) || defined(WIN_X86)
#include <windows.h>

intern WORD GetForegroundColor(console_color color)
{
	switch(color)
	{
		case CONSOLE_BLACK:
			return 0;
		case CONSOLE_GREY:
			return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE;
		case CONSOLE_WHITE:
			return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE | FOREGROUND_INTENSITY;
		case CONSOLE_RED:
			return FOREGROUND_RED | FOREGROUND_INTENSITY;
		case CONSOLE_YELLOW:
			return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_INTENSITY;
	}

	return 0;
}

intern WORD GetBackgroundColor(console_color color)
{
	switch(color)
	{
		case CONSOLE_BLACK:
			return 0;
		case CONSOLE_GREY:
			return BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE;
		case CONSOLE_WHITE:
			return BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE | BACKGROUND_INTENSITY;
		case CONSOLE_RED:
			return BACKGROUND_RED | BACKGROUND_INTENSITY;
		case CONSOLE_YELLOW:
			return BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_INTENSITY;
	}

	return 0;
}

void Log_Console_SetColor(console_color foreground, console_color background)
{
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	WORD color = GetForegroundColor(foreground) | GetBackgroundColor(background);

	SetConsoleTextAttribute(hConsole, color);
}
#endif