@echo off

SET CONFIGURATION=Release

SET BuildDir=.\LazyDataWriter
SET AdditionalsDir=.\Additionals

SET HelperScripts=%AdditionalsDir%\Scripts
SET SetupScripts=%AdditionalsDir%\Setup
SET NuGetDir=.

SET ProjectPaths='%BuildDir%\LazyDataWriter.csproj'

echo.
echo ##### Create Library #####
echo.

CHOICE /C mb /N /M "Shall the [b]uild (x.x._X_.0) or the [m]inor version (x._X_.0.0) be increased?"
SET VERSIONSELECTION=%ERRORLEVEL%
echo.

if /i "%VERSIONSELECTION%" == "1" (
	echo.
	echo Update minor version
	echo.

	powershell "%SetupScripts%\Update_VersionMinor.ps1 -projectPaths %ProjectPaths%"
)

GOTO BUILD

:BUILD

echo.
echo Clean solution
echo.

CALL "%HelperScripts%\Clean.bat"

echo.
echo Build solution
echo.

dotnet build "%BuildDir%\LazyDataWriter.csproj" --configuration %CONFIGURATION%

echo.
echo Test solution
echo.

dotnet test LazyDataWriter.sln --logger:"console;verbosity=detailed"

if not %ERRORLEVEL% == 0 goto BUILDEND

echo.
echo Copy NuGet packages
echo.

del %NuGetDir%\*.nupkg
for /R %cd% %%f in (*.nupkg) do copy %%f %NuGetDir%\

echo.
echo Update build version
echo.

powershell "%SetupScripts%\Update_VersionBuild.ps1 -projectPaths %ProjectPaths%"
goto BUILDEND

:BUILDEND

echo.
PAUSE
