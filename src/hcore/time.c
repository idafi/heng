#include "time.h"

intern time_state currentState;

HEXPORT(void) Time_Delay(uint32 ms)
{
	SDL_Delay(ms);
}

HEXPORT(void) Time_SetTargetFrametime(uint32 ms)
{
	currentState.targetFrametime = ms;
}

HEXPORT(void) Time_DelayToTarget()
{
	if(currentState.targetFrametime > 0)
	{
		int target = (int)(currentState.targetFrametime);
		int diff = target - (int)(currentState.deltaTicks);

		if(diff > 0)
		{ Time_Delay((uint32)(diff)); }
	}
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

HEXPORT(void) Time_GetSnapshot(time_state *state)
{
	uint32 newTicks = SDL_GetTicks();

	currentState = (time_state)
	{
		.deltaTicks = newTicks - currentState.totalTicks,
		.totalTicks = newTicks,

		.targetFrametime = currentState.targetFrametime
	};
}