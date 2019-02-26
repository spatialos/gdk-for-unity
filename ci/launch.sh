#!/usr/bin/env bash

set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ".shared-ci/scripts/profiling.sh"
source ".shared-ci/scripts/pinned-tools.sh"

if [[ -n "${BUILDKITE-}" ]]; then
    # In buildkite, download the artifacts and reconstruct the build/assemblies folder.
    buildkite-agent artifact download "build\assembly\**\*" .
else
    # In TeamCity, just build.
    ci/build-test.sh
fi

uploadAssembly "${ASSEMBLY_PREFIX}" "${PROJECT_NAME}"

markStartOfBlock "Launching deployment"

spatial cloud launch "${ASSEMBLY_NAME}" cloud_launch.json "${ASSEMBLY_NAME}" --snapshot=snapshots/default.snapshot | tee -a ./launch.log

if [[ -n "${BUILDKITE-}" ]]; then
    CONSOLE_REGEX='.*Console URL:(.*)\\n"'
    LAUNCH_LOG=$(cat ./launch.log)
    if [[ $LAUNCH_LOG =~ $CONSOLE_REGEX ]]; then
        CONSOLE_URL=${BASH_REMATCH[1]}
        buildkite-agent annotate --style "success" "Deployment URL: ${CONSOLE_URL}"
    else
        buildkite-agent annotate --style "warning" "Could not parse deployment URL from launch log."
    fi
fi

markEndOfBlock "Launching deployment"
