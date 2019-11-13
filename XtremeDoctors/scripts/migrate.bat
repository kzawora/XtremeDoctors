@ECHO OFF
cd %~dp0\..
set /p name="Migration name: "
dotnet ef migrations add %name% -v
pause;
