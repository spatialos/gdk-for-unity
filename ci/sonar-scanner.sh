#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

cd "$(dirname "$0")/../"

PROJECT_DIR="$(pwd)"

source .shared-ci/scripts/pinned-tools.sh

echo "--- Downloading Buildkite artifacts :buildkite:"

mkdir -p logs/coverage-results

buildkite-agent artifact download \
    logs\\coverage-results\\*.xml \
    logs/coverage-results \
    --step ":windows: ~ test"

TOKEN=$(imp-ci secrets read --environment="production" --buildkite-org="improbable" --secret-type="generic-token" --secret-name="gdk-for-unity-bot-sonarcloud-token" --field="token")

args=()
args+=("-k:spatialos_gdk-for-unity")
args+=("-o:spatialos")
args+=("-d:sonar.login=${TOKEN}")
args+=("-d:sonar.project_key=spatialos_gdk-for-unity")
args+=("-d:sonar.host.url=https://sonarcloud.io")
args+=("-d:sonar.cs.opencover.reportsPaths=../../logs/coverage-results/**/*.xml")
args+=("-d:sonar.buildString=${BUILDKITE_MESSAGE}")
args+=("-d:sonar.log.level=${SONAR_LOG_LEVEL:-"INFO"}")
args+=("-d:sonar.exclusions=Assets/Generated/Source/**/*.cs")

if [[ -n "${DEBUG-}" ]]; then
  args+=("-d:sonar.verbose=true")
fi

if [[ -n "${SONAR_PROJECT_DATE:-}" ]]; then
  # For historical analysis. Note - can only supply a date later than the most recent one in the database.
  args+=("-d:sonar.projectDate=${SONAR_PROJECT_DATE}")
fi

if [[ -n "${SONAR_REVISION:-}" ]]; then
  # Override revision. Useful during historical analysis (i.e. override it to a tag that's being analysed).
  args+=("-d:sonar.scm.revision=${SONAR_REVISION}")
fi

if [[ -n "${BUILDKITE_PULL_REQUEST:-}" && "${BUILDKITE_PULL_REQUEST}" != "false" ]]; then
  # PR analysis, relies on BK building PRs
  # https://docs.sonarqube.org/latest/analysis/pull-request/
  args+=("-d:sonar.pullrequest.key=${BUILDKITE_PULL_REQUEST}")
  args+=("-d:sonar.pullrequest.branch=${BUILDKITE_BRANCH}")
  args+=("-d:sonar.pullrequest.base=${BUILDKITE_PULL_REQUEST_BASE_BRANCH}")
else 
  args+=("-d:sonar.branch.name=${BUILDKITE_BRANCH}")
fi

# Need to generate csproj & sln files in order to run MSBuild on them.
pushd "workers/unity"

    echo "--- Generate csproj & sln files :csharp:"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -projectPath "${PROJECT_DIR}/workers/unity" \
        -quit \
        -executeMethod UnityEditor.SyncVS.SyncSolution
    
    echo "--- Execute sonar-scanner :sonarqube:"
    dotnet-sonarscanner begin "${args[@]}"
    dotnet msbuild ./unity.sln -t:Rebuild -nr:false
    dotnet-sonarscanner end "-d:sonar.login=${TOKEN}"
popd

