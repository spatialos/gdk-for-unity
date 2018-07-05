#!/usr/bin/env bash

cd "$(dirname "$0")"

rm -Rf ../../schema/from_gdk_packages

mkdir ../../schema/from_gdk_packages

[[ -n `find -name "*.schema" -print -quit` ]] && cp `find -name "*.schema"` ../../schema/from_gdk_packages
