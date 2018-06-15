#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

cp hooks/* .git/hooks/

spatial codegen
spatial build build-config
