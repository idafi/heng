# heng
heng - (uncreatively, "h engine," from the working title of the game it targets) - is a programmer-oriented 2D game engine, written in C and C#, with an immutability-centric design.

It currently targets Windows x86 and x64. Linux x86/x64 is planned and perfectly feasible, but pending.

Most large commercial engines feature robust object models, huge high-level APIs, and other helpful systems designed to reduce if not eliminate designer-level programming.
This is great, but it tends to get in the way of a designer not afraid of programming. At best, these overcomplicate the issue, and at worst, they lock out effective and efficient practices for both software and game design.

A custom engine was thus built to help me avoid these problems. (I strongly doubt it will help you until it has many more features, and a few GUI tools.)

---

# Overview

## Structure

The "engine" is simply a C# library assembly -- *heng* -- providing an API for use by a client game executable. (This game executable is probably a C# program, but it could feasibly be anything capable of binding to C# assemblies.) The low-level C layer -- *hcore* -- is used exclusively by the engine, and should be irrelevant and invisible to the client's game. A sample game -- *hgame* -- is also provided, to illustrate the kind of object model the engine systems are designed to enable.

Engine services are modeled as immutable state objects, built using system-relevant subobjects -- Windows and Sprites are used to build a VideoState, for instance. Each service's state object is then meant to be built into a self-contained, immutable master game state. This is a distinct shift in thinking from the way popular engines handle their object models, but provides several advantages in internal security, concurrency support, and system encapsulation.

The game is meant to author its own entry point and frame loop. If the engine's immutable structure isn't correct for the game's needs, the game is free to create whatever wrapper systems it needs in service of its own object model.

## Features

Currently available:
- Input reading and filtering, through string-keyed virtual devices
- Multi-window management and hardware rendering
- Audio loading, playback, mixing, and attenuation, with automatic format conversion
- Collision detection and resolution
- Timekeeping and frametime-targeting
- Event logging and re-simulating
- Virtualized coordinate system

Planned:
- Dynamic region-based resource un/loading
- Lightweight, customizable physics model
- Full gamestate re/serialization
- Tilemap system

---

# Building

You can build on Windows using either the provided Visual Studio 2017 solution, or the provided batch builders.

The C layer requires some external libraries:
- SDL2, as a low-level hardware interface.
- OGG, and
- Vorbis, for loading and reading OGG Vorbis audio.

A NuGet package configuration for these libraries is provided.

### Visual Studio
The C# layer uses .NET Framework 4.7, and several C# 7 features. If you're on older versions of Visual Studio, or a newer version that lacks the appropriate support, you'll need to set that up.

If that's in order, simply acquire the NuGet packages, and you should be good to go.

### Batch builders
The C layer compiles with Clang in C11 mode, but is written to fully support Microsoft Visual C. The MSVC compiler, or any other popular compiler in at least C99 mode, should be able to handle it if you'd prefer to substitute your own.

The C# layer uses several C# 7 features, so you'll need a compiler that can handle those. No .NET Framework 4.7-exclusive features are currently used, but the VS build is configured as a 4.7 project; so, in case such features do find their way in, make sure you're appropriately updated.

You'll need to set up your local environment by altering the path variables in *util/setup_env.bat*. Library dependencies assume that libraries are acquired from NuGet and placed in a *lib/* directory at the project's base path. If you're getting them from somewhere else, you need to set up the appropriate paths in *util/setup.bat*.

If you're using a different C compiler, you'll almost certainly need to rewrite *build_hcore.bat* using the appropriate build options. In such a case, I'll assume you know what you're doing and can take it from there.

Once your directories and libraries are appropriately configured, the *build.bat* script will set them up and start building the project.
