@ECHO OFF
pushd %~dp0\..

REM apply migrations
dotnet ef database update -v

REM pause if called manually
If "%1"=="DoNotPauseAtTheEnd" goto end
    pause
:end
popd
