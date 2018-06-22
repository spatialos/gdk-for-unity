ROOT=`dirname $0`

spatial schema generate -i "${ROOT}/../schema" -L ast_json -o "${ROOT}/../generated_schema"
spatial schema generate -i "${ROOT}/../schema" -L csharp -o "${ROOT}/Generated_Schema"

"${ROOT}/../../bin/Release/GdkCodeGenerator.exe" --json-dir "${ROOT}/../generated_schema" --output-dir "${ROOT}/Generated"