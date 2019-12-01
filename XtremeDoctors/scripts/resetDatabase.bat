@ECHO OFF
pushd %~dp0\..

REM Deleted database
IF EXIST XtremeDoctorsDatabase.db DEL XtremeDoctorsDatabase.db

REM update database
dotnet ef database update -v

REM pause if called manually
If "%1"=="DoNotPauseAtTheEnd" goto end
    pause
:end
popd
