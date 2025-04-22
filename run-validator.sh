#!/bin/bash
echo "🔍 Running SOLTEC.PreBuildValidator..."

dotnet build ../Tools/SOLTEC.PreBuildValidator
if [ $? -ne 0 ]; then
  echo "❌ Build failed."
  exit 1
fi

dotnet ../Tools/SOLTEC.PreBuildValidator/bin/Debug/net8.0/SOLTEC.PreBuildValidator.dll
if [ $? -ne 0 ]; then
  echo "❌ Validator failed."
  exit 1
fi

echo "✅ Validator completed successfully."