#!/usr/bin/env bash

set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

PREFIX="playground"

source ".shared-ci/scripts/profiling.sh"
source ".shared-ci/scripts/pinned-tools.sh"

if [[ -n "${BUILDKITE}" ]]; then
    # In buildkite, download the artifacts and reconstruct the build/assemblies folder.
    buildkite-agent artifact download "build\assembly\**\*" .
else
    # In TeamCity, just build.
    ci/build-test.sh
fi

setAssemblyName "${PREFIX}"

.shared-ci/scripts/upload-assemblies.sh "${ASSEMBLY_NAME}"

markStartOfBlock "Launching deployment"

spatial cloud launch "${ASSEMBLY_NAME}" cloud_launch.json "${ASSEMBLY_NAME}" --snapshot=snapshots/default.snapshot

markEndOfBlock "Launching deployment"
