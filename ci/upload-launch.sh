#!/usr/bin/env bash

set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

PREFIX="playground"

source ".shared-ci/scripts/profiling.sh"
source ".shared-ci/scripts/pinned-tools.sh"

if [[ -n "${BUILDKITE}" ]]; then
    # In buildkite, download the artifacts and reconstruct the build/assemblies folder.
    buildkite-agent artifact download "build\assembly\**\*" .

    # HACK: Since we don't control the schema descriptor right now, we need to fake a build to get all the packages
    # so prepare-for-run does not fail.
    # This is a no-op once it gets to actually building.
    # This can probably be resolved with FPL.
    (export WORKER_TYPE="AndroidClient" && export BUILD_TARGET="cloud" && export SCRIPTING_TYPE="mono" && ./ci/build.sh)

else
    # In TeamCity, just build.
    ci/build-test.sh
fi

setAssemblyName "${PREFIX}"

spatial cloud upload "${ASSEMBLY_NAME}" --log_level=debug --force

markStartOfBlock "Launching deployment"

spatial cloud launch "${ASSEMBLY_NAME}" cloud_launch.json "${ASSEMBLY_NAME}" --snapshot=snapshots/default.snapshot

markEndOfBlock "Launching deployment"
