@ECHO off

IF NOT DEFINED CPU (GOTO envmissing)

ECHO.
ECHO ----------------------
ECHO building hcore
ECHO ----------------------
ECHO.

clang ^
	-shared ^
	-target %ARCH%-windows-unknown ^
	-g -gcodeview ^
	-O0 ^
	-std=c11 ^
	-Wall -Wno-deprecated-declarations ^
	-D%PLTDEF% ^
	-I%HENG_SDL2_INC% -L%HENG_SDL2_LIB% -lSDL2main.lib -lSDL2.lib ^
	-I%HENG_VORBIS_INC% -L%HENG_VORBIS_LIB% -llibogg.lib -llibvorbis.lib -llibvorbisfile.lib ^
	-o"%HENG_OUT%/hcore.dll" ^
	"src\hcore\*.c"

IF ERRORLEVEL 1 GOTO :EOF

ECHO done
GOTO :EOF

:envmissing
ECHO.
ECHO build environment isn't set up - did you forget to call setup.bat?
EXIT /b 1