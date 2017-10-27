#include "input.h"

intern SDL_GameController *controllers[INPUT_CONTROLLERS_MAX];
intern input_state currentState;

intern void OpenController(int index)
{
	AssertIndex(index, INPUT_CONTROLLERS_MAX);

	SDL_GameController *ctr = SDL_GameControllerOpen(index);
	if(ctr)
	{ controllers[index] = ctr; }
	else
	{ LogError("couldn't open controller %i\n\tSDL error: %s", SDL_GetError()); }
}

intern void CloseController(int index)
{
	AssertIndex(index, INPUT_CONTROLLERS_MAX);

	SDL_GameControllerClose(controllers[index]);
	controllers[index] = NULL;
}

bool Input_Init()
{
	if(SDL_InitSubSystem(SDL_INIT_GAMECONTROLLER) >= 0)
	{
		LogNote("SDL gamecontroller successfully initialized");
		LogNote("input successfully initialized");
		return true;
	}
	else
	{ LogFailure("SDL gamecontroller failed to initialize\n\tSDL error: %s", SDL_GetError()); }

	LogFailure("input failed to initialize");
	return false;
}

void Input_Quit()
{
	for(int i = 0; i < INPUT_CONTROLLERS_MAX; i++)
	{ CloseController(i); }

	memset(&currentState, 0, sizeof(input_state));
	SDL_QuitSubSystem(SDL_INIT_GAMECONTROLLER);
}

void Input_KeyEvent(SDL_KeyboardEvent *ev)
{
	AssertPtr(ev);

	bool *key = &currentState.keyboard[ev->keysym.scancode];
	switch(ev->type)
	{
		case SDL_KEYDOWN:
			*key = true;
			break;
		case SDL_KEYUP:
			*key = false;
			break;
	}
}

void Input_MButtonEvent(SDL_MouseButtonEvent *ev)
{
	AssertPtr(ev);

	// stealthily align SDL macros to our mbutton enum
	uint8 buttonID = ev->button - 1;
	bool *button = &currentState.mouse.buttons[buttonID];

	switch(ev->type)
	{
		case SDL_MOUSEBUTTONDOWN:
			*button = true;
			break;
		case SDL_MOUSEBUTTONUP:
			*button = false;
			break;
	}
}

void Input_CButtonEvent(SDL_ControllerButtonEvent *ev)
{
	AssertPtr(ev);
	AssertIndex(ev->which, INPUT_CONTROLLERS_MAX);
	AssertPtr(controllers[ev->which]);

	bool *button = &currentState.controllers[ev->which].buttons[ev->button];
	switch(ev->type)
	{
		case SDL_CONTROLLERBUTTONDOWN:
			*button = true;
			break;
		case SDL_CONTROLLERBUTTONUP:
			*button = false;
			break;
	}
}

void Input_AxisEvent(SDL_ControllerAxisEvent *ev)
{
	AssertPtr(ev);
	AssertIndex(ev->which, INPUT_CONTROLLERS_MAX);
	AssertPtr(controllers[ev->which]);

	short *axis = &currentState.controllers[ev->which].axes[ev->axis];
	*axis = ev->value;
}

void Input_JoystickOpenEvent(SDL_JoyDeviceEvent *ev)
{
	AssertPtr(ev);

	if(ev->which < INPUT_CONTROLLERS_MAX && SDL_IsGameController(ev->which))
	{ OpenController(ev->which); }
}

void Input_JoystickCloseEvent(SDL_JoyDeviceEvent *ev)
{
	AssertPtr(ev);

	if(ev->which < INPUT_CONTROLLERS_MAX && controllers[ev->which])
	{ CloseController(ev->which); }
}

HEXPORT(void) Input_GetSnapshot(input_state *state)
{
	*state = currentState;
}