#!/usr/bin/env bash
set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

cd "$(dirname "$0")/../"

echo "--- Bootstrapping :boot:"
./ci/get-shared-ci.sh

# Download local copy of the SDK packages.
echo "--- Hit init :right-facing_fist::red_button:"
./init.sh
