#!/usr/bin/env bash
set -e -u -o pipefail
if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

cd "$(dirname "$0")/../"

source ci/includes/profiling.sh
source ci/includes/pinned-tools.sh

markStartOfBlock "$0"

ci/lint.sh

markStartOfBlock "Checking Linting Results.."
CHANGES=$(git ls-files -m)

if [ ! -z "$CHANGES" ]; then
    echo "Linting failed. Run ./ci/lint.sh locally and push the fixes."
    exit 1
fi

markEndOfBlock "Checking Linting Results.."
markEndOfBlock "$0"
