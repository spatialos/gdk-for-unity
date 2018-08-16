#!/usr/bin/env bash

set -e -u -o -x pipefail

cd "$(dirname "$0")/../"

ci/codegen.sh
ci/test.sh
ci/build-client.sh
ci/build-gamelogic.sh
