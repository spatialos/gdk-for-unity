#!/usr/bin/env bash

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh 

function buildCodeGenerator
{
    fetchNugetDependencies

    "${MSBUILD}" code_generator/GdkCodeGenerator.sln //property:Configuration=Release //clp:ErrorsOnly //nologo //m 1>&2
    local EXIT_CODE=$?
    return ${EXIT_CODE}
}

function fetchNugetDependencies
{
    "${MSBUILD}" tools/ImpNuget/ImpNuget.csproj //property:Configuration=Release //clp:ErrorsOnly //nologo //m 1>&2

    pushd "code_generator"
        "../bin/ImpNuget/ImpNuget.exe"
    popd
}

buildCodeGenerator 