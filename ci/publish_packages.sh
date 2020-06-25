#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
    set -x
fi

cd "$(dirname "$0")/../"

PKG_ROOT="workers/unity/Packages"

./init.sh

# Re-publish all packages
pushd "${PKG_ROOT}" > /dev/null
    for package_dir in */;
    do
        echo "${package_dir}"
        pushd "${package_dir}" > /dev/null
            # Created package is the last line of output.
            PACKAGE_NAME=$(npm pack | tail -n 1)
            if [[ -n "${DRY_RUN-}" ]]; then
                cloudsmith push npm spatialos/gdk-for-unity "${PACKAGE_NAME}" --republish --dry-run
                # NOTE: The trailing slash on the repository URL is required.
                npm publish "${PACKAGE_NAME}" --registry="https://npm.spatialoschina.com/repository/unity/" --dry-run
            else
                cloudsmith push npm spatialos/gdk-for-unity "${PACKAGE_NAME}" --republish
                # NOTE: The trailing slash on the repository URL is required.
                npm publish "${PACKAGE_NAME}" --registry="https://npm.spatialoschina.com/repository/unity/"
            fi
            rm "${PACKAGE_NAME}"
        popd > /dev/null
    done
popd > /dev/null
