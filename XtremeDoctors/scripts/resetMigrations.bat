@ECHO OFF
pushd %~dp0\..

REM Delete all migrations
IF NOT EXIST Migrations GOTO noMigrations
    echo Deleting migrations...
    cd Migrations
    del /s *.cs
    cd ..
    echo Deleted migrations
:noMigrations
    echo No migrations to delete
:deleted

REM Create new migration
dotnet ef migrations add InitialMigration -v

REM pause if called manually
If "%1"=="DoNotPauseAtTheEnd" goto end
    pause
:end
popd
