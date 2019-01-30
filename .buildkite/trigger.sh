#!/usr/bin/env bash
# https://brevi.link/shell-style
# https://explainshell.com
set -euo pipefail
if [[ -n "${DEBUG-}" ]]; then
  set -x
fi
cd "$(dirname "$0")/../"

# The step-definitions file is uploaded dynamically to preserve ability for historical builds
# vs changes in CI pipeline configuration.

# If docs branch then do docs premerge
if [[ $(git branch | sed -n -e 's/^\* \(.*\)/\1/p') == docs/\* ]]; then
  STEPS=${1/premerge/docs-premerge}
  buildkite-agent pipeline upload "$STEPS"
else 
  buildkite-agent pipeline upload "$1"
fi