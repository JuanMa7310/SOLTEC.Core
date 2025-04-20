#!/bin/bash
echo "🔍 Running SOLTEC.Core.PreBuildValidator..."

dotnet build ../Tools/SOLTEC.Core.PreBuildValidator
if [ $? -ne 0 ]; then
  echo "❌ Build failed."
  exit 1
fi

dotnet ../Tools/SOLTEC.Core.PreBuildValidator/bin/Debug/net8.0/SOLTEC.Core.PreBuildValidator.dll
if [ $? -ne 0 ]; then
  echo "❌ Validator failed."
  exit 1
fi

echo "✅ Validator completed successfully."