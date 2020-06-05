#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
    set -x
fi

cd "$(dirname "$0")/../"

source ".shared-ci/scripts/pinned-tools.sh"

traceStart "Downloading assembly :inbox_tray:"
    buildkite-agent artifact download "build\assembly\**\*" .
traceEnd

uploadAssembly "${ASSEMBLY_PREFIX}" "${PROJECT_NAME}"

traceStart "Launching deployment :airplane_departure:"
    # If the RUNTIME_VERSION env variable is already set, i.e. - through the Buildkite menu
    # then skip reading the file.
    if [[ -n $"{RUNTIME_VERSION:-}" ]]; then
    export RUNTIME_VERSION="$(cat workers/unity/Packages/io.improbable.gdk.tools/runtime.pinned)"
    fi

    dotnet run -p workers/unity/Packages/io.improbable.gdk.deploymentlauncher/.DeploymentLauncher/DeploymentLauncher.csproj -- \
        create \
        --project_name "${PROJECT_NAME}" \
        --assembly_name "${ASSEMBLY_NAME}" \
        --deployment_name "${ASSEMBLY_NAME}" \
        --launch_json_path cloud_launch.json \
        --snapshot_path snapshots/default.snapshot \
        --region EU \
        --tags "dev_login" \
        --runtime_version "${RUNTIME_VERSION}"
traceEnd

CONSOLE_URL="https://console.improbable.io/projects/${PROJECT_NAME}/deployments/${ASSEMBLY_NAME}/overview"

buildkite-agent annotate --style "success" "Deployment URL: ${CONSOLE_URL}<br/>"
