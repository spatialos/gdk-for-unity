#!/usr/bin/env bash

set -e -u -o -x pipefail

cd "$(dirname "$0")/../"

# Get shared CI and prepare Unity
ci/bootstrap.sh

dotnet run -p ./.shared-ci/tools/Packer/Packer.csproj
