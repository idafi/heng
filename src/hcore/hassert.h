#pragma once

#include "_shared.h"

#ifdef NDEBUG

#define Assert(cond, msg) noop
#define AssertPtr(prt) noop
#define AssertIndex(i, len) noop
#define AssertCount(i, ct) noop

#else

#define Assert(cond, msg, ...) \
	if(!(cond)) \
	{ \
		LogFailure("Assertion failed!\n\t%s: %s(): line %u\n\t"msg, __FILE__, __func__, __LINE__, ##__VA_ARGS__); \
		Log_Quit(); \
		abort(); \
	}

#define AssertPtr(ptr) Assert((ptr), "pointer '"#ptr"' is NULL")
#define AssertSign(n) Assert((n) > -1, "value of '"#n"' is negative (%i)", n);
#define AssertIndex(i, len) Assert(((i) > -1) && ((i) < (len)), \
	"array index '"#i"' is invalid: expected greater than -1, less than %i, but got %i", len, i)
#define AssertCount(i, ct) Assert(((i) > -1) && ((i) <= (ct)), \
	"count '"#i"' is invalid: expected greater than -1, no more than %i, but got %i", ct, i)

#endif