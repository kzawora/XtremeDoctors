@ECHO OFF
cd %~dp0\..
IF EXIST XtremeDoctorsDatabase.db DEL XtremeDoctorsDatabase.db
dotnet ef database update -v
pause;
