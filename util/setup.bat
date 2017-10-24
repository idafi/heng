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

REM - set platform args for compiler
if %CPU% == x86 ( 
	SET PLTDEF="WIN_X86" 
	SET ARCH="i386"
) ELSE ( 
	SET PLTDEF="WIN_X64"
	SET ARCH="x86_64"
)

REM - set up lib dependencies
SET HENG_SDL2=%HENG_HOME%\lib\SDL2-2.0.5
SET HENG_VORBIS=%HENG_HOME%\lib\vorbis
SET HENG_JSONNET=%HENG_HOME%\lib\Newtonsoft.Json-10.0.3\Bin\net45

SET HENG_SDL2_INC=%HENG_SDL2%\include
SET HENG_VORBIS_INC=%HENG_VORBIS%\include

SET HENG_SDL2_LIB=%HENG_SDL2%\lib\%CPU%
SET HENG_VORBIS_LIB=%HENG_VORBIS%\lib\%CPU%

SET HENG_SDL2_DLL=%HENG_SDL2%\bin\%CPU%\SDL2.dll
SET HENG_JSONNET_DLL=%HENG_JSONNET%\Newtonsoft.Json.dll

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