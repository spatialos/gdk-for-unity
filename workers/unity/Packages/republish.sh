#!/usr/bin/env bash

set -e -u -o pipefail

cd "$(dirname "$0")"

for dir in */;
do
    echo $dir
    pushd $dir
        npm unpublish --registry=http://localhost:4873 --force
        npm publish --registry=http://localhost:4873
    popd
done

rm -rf "$LOCALAPPDATA/Unity/cache/npm/localhost_4873"
rm -rf "$LOCALAPPDATA/Unity/cache/packages/localhost_4873"