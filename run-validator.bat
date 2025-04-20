@echo off
echo Running SOLTEC.Core.PreBuildValidator...

dotnet build ..\Tools\SOLTEC.Core.PreBuildValidator
if %ERRORLEVEL% NEQ 0 (
    echo Build failed.
    exit /b %ERRORLEVEL%
)

dotnet ..\Tools\SOLTEC.Core.PreBuildValidator\bin\Debug\net8.0\SOLTEC.Core.PreBuildValidator.dll
if %ERRORLEVEL% NEQ 0 (
    echo Validator failed.
    exit /b %ERRORLEVEL%
)

echo Validator completed successfully.
pause