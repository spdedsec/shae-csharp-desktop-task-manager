#!/usr/bin/env bash
# scripts/install.sh — shae-csharp-desktop-task-manager
#
# SHAE Task Manager — a console-based task manager in C# (.NET).
#
# Requires the .NET SDK. Not available for compile-testing in this
# environment (only the .NET runtime is present, no SDK); see README.md.

set -euo pipefail

echo "=== shae-csharp-desktop-task-manager install ==="
echo ""

echo "Requirements:"
echo "  - .NET SDK 6.0+ (dotnet)"
echo ""

if command -v dotnet >/dev/null 2>&1; then
  echo "dotnet found: $(dotnet --list-sdks 2>/dev/null || echo 'runtime only, no SDK')"
else
  echo "dotnet not found on PATH. Install the .NET SDK:"
  echo "  https://dotnet.microsoft.com/download"
  echo "  Debian/Ubuntu: see https://learn.microsoft.com/dotnet/core/install/linux-ubuntu"
  echo "  macOS:         brew install --cask dotnet-sdk"
  echo "  Arch:          sudo pacman -S dotnet-sdk"
fi

echo ""
echo "=== Build & run ==="
echo "# From the project root:"
echo "  cd shae-csharp-desktop-task-manager/src"
echo "  dotnet build"
echo "  dotnet run"
echo ""
echo "Commands: list, todo, done, add, toggle <id>, delete <id>, help, quit"
echo "Tasks are saved to ~/.shae-task-manager/tasks.json"
