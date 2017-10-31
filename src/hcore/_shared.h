#pragma once

// shared includes
#include <float.h>
#include <limits.h>
#include <malloc.h>
#include <math.h>
#include <stdbool.h>
#include <stddef.h>
#include <stdlib.h>
#include <stdint.h>
#include <stdio.h>
#include <string.h>
#include <SDL.h>

// keyword re/definitions
#define noop ((void)0)
#define intern static
typedef int8_t sbyte;
typedef int8_t int8;
typedef int16_t int16;
typedef int32_t int32;
typedef int64_t int64;
typedef uint8_t byte;
typedef uint8_t uint8;
typedef uint16_t uint16;
typedef uint32_t uint32;
typedef uint64_t uint64;

// compiler specifics
#ifdef _MSC_VER
#define HINLINE intern __inline
#elif __GNUC__
#define HINLINE intern inline
#endif

// platform specifics
#ifdef WIN_X64
#define HEXPORT(type) __declspec(dllexport) type __cdecl
#elif WIN_X86
#define HEXPORT(type) __declspec(dllexport) type __stdcall
#else
#define HEXPORT(type) type	// linux
#endif

// math macros
#undef M_PI	// SDL wants to define this for us? we'll use our own instead
#define M_PI 3.14159265358979323846

#define min(a, b) (((a) < (b)) ? (a) : (b))
#define max(a, b) (((a) > (b)) ? (a) : (b))
#define Clamp(value, lower, upper) (min(max((value), (lower)), (upper)))

// bitfield helpers
#define FlagSet(mask, flag) ((mask) |= (flag))
#define FlagClear(mask, flag) ((mask) &= ~(flag))
#define FlagTest(mask, flag) (((mask) & (flag)) == (flag))

// shared hcore includes
#include "hassert.h"
#include "log.h"
#include "vector.h"