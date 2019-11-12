@ECHO OFF
cd %~dp0\..
dotnet ef database update -v
pause;
