@ECHO off

REM - don't waste time if already set up
IF DEFINED CPU (
	IF "%1" == "" (GOTO skip)
	IF "%CPU%" == "%1" (GOTO skip)
)

ECHO.
ECHO ----------------------
ECHO setting up build environment
ECHO ----------------------
ECHO.

REM - get target cpu
IF "%1" == "" (SET CPU=x64) ELSE (SET CPU=%1)
IF NOT %CPU% == x64 IF NOT %CPU% == x86 GOTO badcpu

ECHO targeting win %CPU%

REM - get user environment
CALL util\setup_env.bat
IF ERRORLEVEL 1 GOTO fail

REM - user environment check
IF NOT DEFINED HENG_HOME (GOTO envmissing)
IF NOT DEFINED HENG_LLVM (GOTO envmissing)
IF NOT DEFINED HENG_DOTNET (GOTO envmissing)

REM - set output dir
SET HENG_OUT=%HENG_HOME%\bin\%CPU%

REM - set other platform aliases
if %CPU% == x86 ( 
	SET ARCH="i386"
	SET PLATFORM=Win32
) ELSE ( 
	SET ARCH="x86_64"
	SET PLATFORM=x64
)

REM - set up lib dependencies
SET HENG_SDL2_INC=%HENG_HOME%\lib\sdl2.2.0.5\build\native\include
SET HENG_SDL2_LIB=%HENG_HOME%\lib\sdl2.2.0.5\build\native\lib\%PLATFORM%\dynamic
SET HENG_SDL2_DLL=%HENG_HOME%\lib\sdl2.redist.2.0.5\build\native\bin\%PLATFORM%\dynamic

SET HENG_OGG_INC=%HENG_HOME%\lib\ogg-msvc-%CPU%.1.3.2.8787\build\native\include
SET HENG_OGG_LIB=%HENG_HOME%\lib\ogg-msvc-%CPU%.1.3.2.8787\build\native\lib_release

SET HENG_VORBIS_INC=%HENG_HOME%\lib\vorbis-msvc-%CPU%.1.3.5.8787\build\native\include
SET HENG_VORBIS_LIB=%HENG_HOME%\lib\vorbis-msvc-%CPU%.1.3.5.8787\build\native\lib_release

ECHO done
GOTO :EOF

:envmissing
ECHO One or more environment variables aren't set -- did you add the right paths to setup_env?
ECHO HENG_HOME is %HENG_HOME%
ECHO HENG_LLVM is %HENG_LLVM%
ECHO HENG_DOTNET is %HENG_DOTNET%
GOTO fail

:badcpu
ECHO invalid cpu target specified ('%1'); expected x64 or x86
GOTO fail

:fail
EXIT /b 1

:skip
ECHO.
ECHO already targeting win %CPU% - skipping environment setup
EXIT /b 0