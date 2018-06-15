#!/usr/bin/env bash

if [ $# -ne 1 ]; then
  echo "Usage: deploy.sh <deployment_name_with_underscores>"
  exit 1
fi

set -e -u -x -o pipefail

cd "$(dirname "$0")/"

spatial cloud upload $1_assembly --force
spatial cloud launch \
  $1_assembly \
  cloud_launch.json \
  $1_deployment \
  --snapshot=snapshots/default.snapshot \
  --cluster_region=eu
