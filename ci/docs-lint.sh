#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

function isDocsBranch() {
  if [[ -n "${BUILDKITE-}" ]]; then
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
  else
    BRANCH="${BUILDKITE_BRANCH}"
  fi

  if [[ "${BRANCH}" == docs/* ]]; then
    return 0
  fi
  return 1
}

if [[ ! isDocsBranch ]]; then
    exit 0
fi

ci/bootstrap.sh

source .shared-ci/scripts/pinned-tools.sh
source .shared-ci/scripts/profiling.sh

markStartOfBlock "Running Docs Linter"
dotnet run -p .shared-ci/tools/DocsLinter/DocsLinter.csproj 
markEndOfBlock "Running Docs Linter"