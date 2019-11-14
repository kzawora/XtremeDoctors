@ECHO OFF
pushd %~dp0\..

REM Add migration
set /p name="Migration name: "
dotnet ef migrations add %name% -v

REM pause if called manually
If "%1"=="DoNotPauseAtTheEnd" goto end
    pause
:end
popd
