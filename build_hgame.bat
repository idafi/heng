@ECHO off

IF NOT DEFINED CPU (GOTO envmissing)

ECHO.
ECHO ----------------------
ECHO building hgame
ECHO ----------------------
ECHO.

%HENG_DOTNET%\csc.exe -nologo ^
	-debug -d:DEBUG ^
	-platform:%CPU% -t:exe ^
	-r:%HENG_OUT%\heng.dll ^
	-r:%HENG_JSONNET_DLL% ^
	-out:"%HENG_OUT%\hgame.exe" ^
	-recurse:"src\hgame\*.cs"

IF ERRORLEVEL 1 GOTO :EOF

ECHO done
GOTO :EOF

:envmissing
ECHO.
ECHO build environment isn't set up - did you forget to call setup.bat?

EXIT /b 1