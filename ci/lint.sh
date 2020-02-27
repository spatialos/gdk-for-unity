#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

cd "$(dirname "$0")/.."

./ci/get-shared-ci.sh
./.shared-ci/scripts/lint.sh ./workers/unity --check
