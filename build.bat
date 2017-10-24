@ECHO off

CALL util\setup.bat %*
IF ERRORLEVEL 1 GOTO fail

REM - ensure output dir
IF NOT EXIST %HENG_OUT% (MKDIR %HENG_OUT%)

CALL build_hcore.bat
IF ERRORLEVEL 1 GOTO fail

CALL build_heng.bat
IF ERRORLEVEL 1 GOTO fail

CALL build_hgame.bat
IF ERRORLEVEL 1 GOTO fail

ECHO.
ECHO ----------------------
ECHO copying dependencies
ECHO ----------------------
ECHO.

XCOPY /Q /Y %HENG_SDL2_DLL% %HENG_OUT%
IF ERRORLEVEL 1 GOTO fail

XCOPY /Q /Y %HENG_JSONNET_DLL% %HENG_OUT%
IF ERRORLEVEL 1 GOTO fail

ECHO.
ECHO ----------------------
ECHO cleaning garbage
ECHO ----------------------
ECHO.

DEL %HENG_OUT%\*.ilk
DEL %HENG_OUT%\*.lib
DEL %HENG_OUT%\*.exp
DEL %HENG_OUT%\*.tmp

ECHO done

ECHO.
ECHO build successful!
GOTO :EOF

:fail
ECHO.
ECHO build failed