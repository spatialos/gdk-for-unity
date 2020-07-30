#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
    set -x
fi

cd "$(dirname "$0")/../"

PROJECT_DIR="$(pwd)"

source .shared-ci/scripts/pinned-tools.sh

# TODO: Add a more specific secret-type to vault and swap these around
TOKEN=$(imp-ci secrets read --environment="production" --buildkite-org="improbable" --secret-type="generic-token" --secret-name="gdk-for-unity-bot-sonarcloud-token" --field="token")

args=()
args+=("-k:spatialos_gdk-for-unity")
args+=("-o:spatialos")
args+=("-d:sonar.login=${TOKEN}")
args+=("-d:sonar.project_key=spatialos_gdk-for-unity")
args+=("-d:sonar.host.url=https://sonarcloud.io")
args+=("-d:sonar.buildString=${BUILDKITE_MESSAGE:0:100}")
args+=("-d:sonar.log.level=${SONAR_LOG_LEVEL:-"INFO"}")

# Exclude all generated code from analysis.
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
    # Branch analysis, allows for diff-level reporting on short-lived branches
    # https://docs.sonarqube.org/latest/branches/overview/
    args+=("-d:sonar.branch.name=${BUILDKITE_BRANCH}")
fi

# The way Sonar Scanner for MSBuild/dotnet works is:
#   1. You need to run `dotnet-sonarscanner begin` which installs hooks into MSBuild and applies your quality profiles (from sonarcloud.io)
#   2. Build the project using `msbuild`
#   3. Run `dotnet-sonarscanner end` which runs post-processing. (This looks like it runs an embedded version of the generic sonar-scanner CLI which requires the JRE).
#
# To enable this to work with Unity, we first need to generate the `.sln` and `.csproj` files in order to build them with `msbuild`
# For ease of use, we execute msbuild through `dotnet`!
pushd "workers/unity"
    # This finds all .xml files in the logs/ directory and concatentates their relative path together, separated by comma:
    # E.g. - -d:sonar.cs.opencover.reportsPath=./logs/coverage-results/my-first-report.xml,./logs/coverage-results/my-second-report.xml
    # Wildcards don't appear to play nice with this.
    args+=("-d:sonar.cs.opencover.reportsPaths=$(find ./logs -name "*.xml" | paste -sd "," -)")

    dotnet new globaljson --sdk-version 2.2.402 --force 
    traceStart "Execute sonar-scanner :sonarqube:"
        dotnet-sonarscanner begin "${args[@]}"
        dotnet msbuild ./unity.sln -t:Rebuild -nr:false | tee "${PROJECT_DIR}/logs/sonar-msbuild.log"
        dotnet-sonarscanner end "-d:sonar.login=${TOKEN}" | tee "${PROJECT_DIR}/logs/sonar-postprocess.log"
    traceEnd
popd
