@echo off
setlocal
cd /d "%~dp0"
set "DOTNET_ROOT=%USERPROFILE%\.dotnet"
set "PATH=%DOTNET_ROOT%;%PATH%"
if not exist "%DOTNET_ROOT%\dotnet.exe" (
  echo .NET 8 SDK not found at %DOTNET_ROOT%
  pause
  exit /b 1
)
"%DOTNET_ROOT%\dotnet.exe" run --project src\DuskOfEmpire.Client
if errorlevel 1 pause
