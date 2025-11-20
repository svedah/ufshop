@REM echo %DATE%
@REM echo %TIME%
@REM set datetimef=%date:~-4%_%date:~3,2%_%date:~0,2%__%time:~0,2%_%time:~3,2%_%time:~6,2%
set datetimef=%date%_%time:~0,2%%time:~3,2%
@REM echo %datetimef%
dotnet ef migrations add %datetimef%
dotnet ef database update