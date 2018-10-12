#!/usr/bin/env bash

set -e -u -o -x pipefail

cd "$(dirname "$0")/../"

# Get shared CI and prepare unity
ci/bootstrap.sh
.shared-ci/scripts/prepare-unity.sh

ci/test.sh
ci/build-client.sh
ci/build-gamelogic.sh
