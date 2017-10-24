@ECHO off

IF NOT DEFINED CPU (GOTO envmissing)

ECHO.
ECHO ----------------------
ECHO building heng
ECHO ----------------------
ECHO.

%HENG_DOTNET%\csc.exe -nologo ^
	-debug -d:DEBUG ^
	-platform:%CPU% -t:library ^
	-out:"%HENG_OUT%\heng.dll" ^
	-recurse:"src\heng\*.cs"

IF ERRORLEVEL 1 GOTO :EOF

ECHO done
GOTO :EOF

:envmissing
ECHO.
ECHO build environment isn't set up - did you forget to call setup.bat?

EXIT /b 1