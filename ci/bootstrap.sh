#!/usr/bin/env bash
set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
    set -x
fi

cd "$(dirname "$0")/../"

echo "## imp-ci group-start Bootstrapping :boot:"

./ci/get-shared-ci.sh
source ".shared-ci/scripts/pinned-tools.sh"

# Download local copy of the SDK packages.
traceStart "Hit init :right-facing_fist::red_button:"
    ./init.sh
traceEnd

echo "## imp-ci group-start Bootstrapping :boot:"
