#include "video.h"

// - - - - - -
// command structs
// - - - - - -

typedef struct
{
	char title[VIDEO_WINDOWS_TITLE_MAX];
	screen_rect rect;

	uint32 windowFlags;
	uint32 rendererFlags;
} vid_command_open;

typedef struct
{
	screen_rect window;
	screen_rect viewport;
} vid_command_mode;

#define VIDEO_MAX_POINTS 1024
typedef struct
{
	screen_point points[VIDEO_MAX_POINTS];
	int pointCount;
} vid_command_points;

typedef struct
{
	screen_line line;
} vid_command_line;

#define VIDEO_MAX_POLYGON_POINTS 64
typedef struct
{
	screen_point points[VIDEO_MAX_POLYGON_POINTS];
	int pointCount;
} vid_command_polygon;

typedef struct
{
	int textureID;
	screen_point position;
	float rotation;
} vid_command_texture;

typedef struct
{
	vid_command_type type;
	color color;

	union
	{
		vid_command_open open;
		vid_command_mode mode;
		vid_command_points points;
		vid_command_line line;
		vid_command_polygon polygon;
		vid_command_texture texture;
	};
} vid_command;

// - - - - - -
// render queue
// - - - - - -

typedef struct
{
	vid_command queue[VIDEO_QUEUE_SIZE];
	int head;
} render_queue;

intern render_queue renderQueues[VIDEO_WINDOWS_MAX];

intern vid_command *QueueNextFreeCommand(int windowID)
{
	render_queue *queue = &renderQueues[windowID];
	if(queue->head < VIDEO_QUEUE_SIZE)
	{
		// default to white
		vid_command *c = &queue->queue[queue->head++];
		c->color = COLOR_WHITE;

		return c;
	}
	else
	{ LogError("can't queue any more render elements for window %i (limit: %i)", windowID, VIDEO_QUEUE_SIZE); }

	return NULL;
}

HEXPORT(void) Video_Queue_OpenWindow(int windowID, char *title, screen_rect rect, uint32 windowFlags, uint32 rendererFlags)
{
	vid_command *c = QueueNextFreeCommand(windowID);
	if(c)
	{
		c->type = VID_COMMAND_OPEN;
		c->open = (vid_command_open)
		{
			.rect = rect,
			.windowFlags = windowFlags,
			.rendererFlags = rendererFlags
		};

		strcpy(c->open.title, title);
	}
}

HEXPORT(void) Video_Queue_CloseWindow(int windowID)
{
	vid_command *c = QueueNextFreeCommand(windowID);
	if(c)
	{ c->type = VID_COMMAND_CLOSE; }
}

HEXPORT(void) Video_Queue_ChangeWindowMode(int windowID, screen_rect window, screen_rect viewport)
{
	vid_command *c = QueueNextFreeCommand(windowID);
	if(c)
	{
		c->type = VID_COMMAND_MODE;
		c->mode.window = window;
		c->mode.viewport = viewport;
	}
}

HEXPORT(void) Video_Queue_ClearWindow(int windowID, color color)
{
	vid_command *c = QueueNextFreeCommand(windowID);
	if(c)
	{
		c->type = VID_COMMAND_CLEAR;
		c->color = color;
	}
}

HEXPORT(void) Video_Queue_DrawPoint(int windowID, color color, screen_point point)
{
	// DrawPoints does a memcpy, so we can safely get away with just passing a reference to this point
	Video_Queue_DrawPoints(windowID, color, &point, 1);
}

HEXPORT(void) Video_Queue_DrawLine(int windowID, color color, screen_line line)
{
	vid_command *c = QueueNextFreeCommand(windowID);
	if(c)
	{
		c->type = VID_COMMAND_LINE;
		c->color = color;

		c->line.line = line;
	}
}

HEXPORT(void) Video_Queue_DrawPoints(int windowID, color color, screen_point *points, int count)
{
	if(points)
	{
		if(count > -1 && count < VIDEO_MAX_POINTS)
		{
			vid_command *c = QueueNextFreeCommand(windowID);
			if(c)
			{
				c->type = VID_COMMAND_POINTS;
				c->color = color;

				// who knows what might happen to the points array between now and render. better copy it
				memcpy(c->points.points, points, sizeof(screen_point) * count);
				c->points.pointCount = count;
			}
		}
		else
		{ LogError("can't queue points: point count (%i) is invalid (max: %i)", count, VIDEO_MAX_POINTS); }
	}
	else
	{ LogError("can't queue points: points array is NULL"); }
}

HEXPORT(void) Video_Queue_DrawPolygon(int windowID, color color, screen_point *points, int count)
{
	if(points)
	{
		if(count > -1 && count < VIDEO_MAX_POINTS)
		{
			vid_command *c = QueueNextFreeCommand(windowID);
			if(c)
			{
				c->type = VID_COMMAND_POLYGON;
				c->color = color;

				// who knows what might happen to the points array between now and render. better copy it
				memcpy(c->polygon.points, points, sizeof(screen_point) * count);

				// polygon needs to draw an extra line connecting the last point to the first
				c->polygon.pointCount = count;
				c->polygon.points[c->polygon.pointCount++] = c->polygon.points[0];
			}
		}
		else
		{ LogError("can't queue polygon: point count (%i) is invalid (max: %i)", count, VIDEO_MAX_POINTS); }
	}
	else
	{ LogError("can't queue polygon: points array is NULL"); }
}

HEXPORT(void) Video_Queue_DrawTexture(int windowID, int textureID, screen_point position, float rotation)
{
	if(Video_Textures_CheckTexture(textureID))
	{
		vid_command *c = QueueNextFreeCommand(windowID);
		if(c)
		{
			// TODO: texture tinting
			c->type = VID_COMMAND_TEXTURE;

			c->texture.textureID = textureID;
			c->texture.position = position;
			c->texture.rotation = rotation;
		}
	}
	else
	{ LogError("can't queue texture: texture with ID %i is invalid", textureID); }
}

HEXPORT(void) Video_Queue_Pump(int windowID)
{
	if(Video_Windows_CheckWindow(windowID))
	{
		render_queue *queue = &renderQueues[windowID];

		// don't bother if there's nothing to do
		if(queue->head > 0)
		{
			// execute queued commands
			for(int i = 0; i < queue->head; i++)
			{
				vid_command *c = &queue->queue[i];
				switch(c->type)
				{
					case VID_COMMAND_INVALID:
						LogError("tried to execute an invalid command");
						break;
					case VID_COMMAND_OPEN:
						Video_Windows_OpenWindow(windowID, c->open.title, c->open.rect, c->open.windowFlags, c->open.rendererFlags);
						break;
					case VID_COMMAND_CLOSE:
						Video_Windows_CloseWindow(windowID);
						break;
					case VID_COMMAND_CLEAR:
						Video_Windows_SetWindowColor(windowID, c->color);
						Video_Windows_ClearWindow(windowID);
						break;
					case VID_COMMAND_MODE:
						// TODO
						break;
					case VID_COMMAND_POINTS:
						Video_Windows_SetWindowColor(windowID, c->color);
						Video_Windows_DrawPoints(windowID, POINTS_DRAW_POINTS, c->points.points, c->points.pointCount);
						break;
					case VID_COMMAND_LINE:
						Video_Windows_SetWindowColor(windowID, c->color);
						Video_Windows_DrawLine(windowID, c->line.line);
						break;
					case VID_COMMAND_POLYGON:
						Video_Windows_SetWindowColor(windowID, c->color);
						Video_Windows_DrawPoints(windowID, POINTS_DRAW_LINES, c->polygon.points, c->polygon.pointCount);
						break;
					case VID_COMMAND_TEXTURE:
						Video_Windows_DrawTexture(windowID, c->texture.textureID, c->texture.position, c->texture.rotation);
						break;
				}
			}

			// present anything newly drawn
			Video_Windows_PresentWindow(windowID);
		}

		Video_Queue_ClearQueue(windowID);
	}
	else
	{ LogError("can't pump queue for window %i: window is invalid", windowID); }
}

HEXPORT(void) Video_Queue_ClearQueue(int windowID)
{
	if(Video_Windows_CheckWindow(windowID))
	{
		render_queue *queue = &renderQueues[windowID];
		memset(queue->queue, 0, sizeof(vid_command) * queue->head);
		queue->head = 0;
	}
	else
	{ LogError("can't clear queue for window %i: window is invalid", windowID); }
}

HEXPORT(void) Video_Queue_PumpAll()
{
	for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
	{ Video_Queue_Pump(i); }
}

HEXPORT(void) Video_Queue_ClearAll()
{
	for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
	{ Video_Queue_ClearQueue(i); }
}

struct video_queue_state Video_Queue_GetSnapshot()
{
	struct video_queue_state state;
	for(int i = 0; i < VIDEO_WINDOWS_MAX; i++)
	{
		state.queues[i].commandCount = renderQueues[i].head;

		for(int c = 0; c < renderQueues[i].head; c++)
		{ state.queues[i].commands[c] = renderQueues[i].queue[c].type; }
	}

	return state;
}