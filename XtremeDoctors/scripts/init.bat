@ECHO OFF
cd %~dp0
call resetMigrations.bat DoNotPauseAtTheEnd
call resetDatabase.bat DoNotPauseAtTheEnd
pause
