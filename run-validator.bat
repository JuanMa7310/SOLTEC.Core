@echo off
echo Running SOLTEC.PreBuildValidator...

dotnet build ..\Tools\SOLTEC.PreBuildValidator
if %ERRORLEVEL% NEQ 0 (
    echo Build failed.
    exit /b %ERRORLEVEL%
)

dotnet ..\Tools\SOLTEC.PreBuildValidator\bin\Debug\net8.0\SOLTEC.PreBuildValidator.dll
if %ERRORLEVEL% NEQ 0 (
    echo Validator failed.
    exit /b %ERRORLEVEL%
)

echo Validator completed successfully.
pause