#!/usr/bin/env bash

set -e -u -o -x pipefail

cd "$(dirname "$0")/../"

PREFIX="playground"

ci/build-test.sh

source ".shared-ci/scripts/profiling.sh"

.shared-ci/scripts/upload-assemblies.sh ${PREFIX}

markStartOfBlock "Launching deployment"

# Get first 8 characters of current git hash.
GIT_HASH="$(git rev-parse HEAD | cut -c1-8)"

spatial cloud launch "${PREFIX}_${GIT_HASH}" cloud_launch.json "${PREFIX}_${GIT_HASH}" --snapshot=snapshots/default.snapshot

markEndOfBlock "Launching deployment"
