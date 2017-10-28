#pragma once

#include "_shared.h"

typedef struct
{
	float x, y;
} vector2;

#define VECTOR2_ZERO ((vector2) { 0, 0 })
#define VECTOR2_RIGHT ((vector2) { 1, 0 })
#define VECTOR2_UP ((vector2) { 0, 1 })
#define VECTOR2_LEFT ((vector2) { -1, 0 })
#define VECTOR2_DOWN ((vector2) { 0, -1 })

HINLINE vector2 VectorAdd(vector2 a, vector2 b)
{
	vector2 v = { a.x + b.x, a.y + b.y };
	return v;
}

HINLINE vector2 VectorSubtract(vector2 a, vector2 b)
{
	vector2 v = { a.x - b.x, a.y - b.y };
	return v;
}

HINLINE vector2 VectorScale(vector2 v, float s)
{
	vector2 sc = { v.x * s, v.y * s };
	return sc;
}

HINLINE vector2 VectorNegate(vector2 v)
{
	vector2 ng = VectorScale(v, -1);
	return ng;
}

HINLINE vector2 VectorClamp(vector2 value, vector2 lower, vector2 upper)
{
	float x = Clamp(value.x, lower.x, upper.x);
	float y = Clamp(value.y, lower.y, upper.y);

	vector2 v = { x, y };
	return v;
}

HINLINE bool VectorEquals(vector2 a, vector2 b)
{
	return (a.x == b.x) && (a.y == b.y);
}

HINLINE float VectorDot(vector2 a, vector2 b)
{
	return (a.x * b.x) + (a.y * b.y);
}

HINLINE float VectorCross(vector2 a, vector2 b)
{
	return (a.x * b.y) - (a.y * b.x);
}

HINLINE float VectorSqrMagnitude(vector2 v)
{
	return VectorDot(v, v);
}

HINLINE float VectorMagnitude(vector2 v)
{
	return sqrtf(VectorSqrMagnitude(v));
}

HINLINE vector2 VectorClampMagnitude(vector2 v, float min, float max)
{
	float mag = VectorMagnitude(v);
	float s = 1;

	if(mag > 0)
	{
		if(mag < min)
		{ s = min / mag; }
		if(mag > max)
		{ s = max / mag; }
	}
	
	return VectorScale(v, s);
}


HINLINE vector2 VectorNormalize(vector2 v)
{
	float mag = VectorMagnitude(v);
	
	if(mag > 0)
	{
		float s = 1 / mag;
		return VectorScale(v, s);
	}
	
	return v;
}

HINLINE vector2 VectorProject(vector2 a, vector2 b)
{
	float dot = VectorDot(a, b);
	float x = (dot / VectorSqrMagnitude(b)) * b.x;
	float y = (dot / VectorSqrMagnitude(b)) * b.y;

	vector2 v = { x, y };
	return v;
}

HINLINE vector2 VectorRightNormal(vector2 v)
{
	vector2 n = { -v.y, v.x };
	return n;
}

HINLINE vector2 VectorLeftNormal(vector2 v)
{
	vector2 n = { v.y, -v.x };
	return n;
}

HINLINE vector2 VectorReflect(vector2 v, vector2 axis)
{
	vector2 normal = VectorRightNormal(axis);
	float dot = VectorDot(v, normal);

	normal = VectorScale(normal, dot * 2);
	return VectorSubtract(v, normal);
}

HINLINE vector2 VectorLerp(vector2 a, vector2 b, float t)
{
	t = Clamp(t, 0, 1);
	
	vector2 v = VectorSubtract(b, a);
	v = VectorScale(v, t);

	return VectorAdd(a, v);
}

HINLINE float VectorToAngle(vector2 v)
{
	return (float)(atan2(v.y, v.x) * (180.0f / M_PI));
}

HINLINE vector2 AngleToVector(float degrees)
{
	float rad = (float)(degrees / (180.0f / M_PI));
	vector2 v = { (float)(cos(rad)), (float)(sin(rad)) };
	
	return v;
}