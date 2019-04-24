#!/usr/bin/env bash

set -e -u -o pipefail

cd "$(dirname "$0")"

# remove local unity cache
rm -rf "$LOCALAPPDATA/Unity/cache/npm"
rm -rf "$LOCALAPPDATA/Unity/cache/packages"