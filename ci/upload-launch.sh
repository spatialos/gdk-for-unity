#!/usr/bin/env bash

set -e -u -o -x pipefail

cd "$(dirname "$0")/../"

PREFIX="playground"

ci/build-test.sh

source ".shared-ci/scripts/profiling.sh"
source ".shared-ci/scripts/pinned-tools.sh"

setAssemblyName "${PREFIX}"

.shared-ci/scripts/upload-assemblies.sh "${ASSEMBLY_NAME}"

markStartOfBlock "Launching deployment"

spatial cloud launch "${ASSEMBLY_NAME}" cloud_launch.json "${ASSEMBLY_NAME}" --snapshot=snapshots/default.snapshot

markEndOfBlock "Launching deployment"
