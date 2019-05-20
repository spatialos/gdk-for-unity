#!/usr/bin/env bash

set -e -u -o pipefail

cd "$(dirname "$0")"

PKG_ROOT="../../workers/unity/Packages"
REGISTRY="https://npm.cloudsmith.io/testorg-3/gdk-for-unity/"

# Re-publish all packages
pushd $PKG_ROOT
    for dir in */;
    do
        echo $dir
        pushd $dir
            npm publish --registry="${REGISTRY}"
        popd
    done
popd
