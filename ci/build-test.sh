#!/usr/bin/env bash

set -e -u -o pipefail

cd "$(dirname "$0")/../"

ci/test.sh
ci/build-client.sh
ci/build-gamelogic.sh
