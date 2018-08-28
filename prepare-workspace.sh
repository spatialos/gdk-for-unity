#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/"

cp hooks/* .git/hooks/

spatial build build-config
