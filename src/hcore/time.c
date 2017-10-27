#include "time.h"

HEXPORT(uint32) Time_GetTicks()
{
	return SDL_GetTicks();
}

HEXPORT(void) Time_Delay(uint32 ms)
{
	SDL_Delay(ms);
}

HEXPORT(double) Time_TimeProcedure(void(*proc)())
{
	if(proc)
	{
		uint64 start, end;
		double freq = (double)(SDL_GetPerformanceFrequency() / 1000);

		start = SDL_GetPerformanceCounter();
		proc();
		end = SDL_GetPerformanceCounter();

		return (double)(end - start) / freq;
	}

	LogError("couldn't time procedure: procedure is null");
	return 0;
}