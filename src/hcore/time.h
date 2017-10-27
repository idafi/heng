#pragma once

#include "_shared.h"

HEXPORT(uint32) Time_GetTicks();
HEXPORT(void) Time_Delay(uint32 ms);

HEXPORT(double) Time_TimeProcedure(void(*proc)());