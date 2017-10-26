#pragma once

#include "_shared.h"

HEXPORT(void) Time_Delay(uint32 ms);

HEXPORT(void) Time_SetTargetFrametime(uint32 ms);
HEXPORT(void) Time_DelayToTarget();

HEXPORT(double) Time_TimeProcedure(void(*proc)());

typedef struct
{
	uint32 totalTicks;
	uint32 deltaTicks;

	uint32 targetFrametime;
} time_state;

HEXPORT(void) Time_GetSnapshot(time_state *state);