ROOT=`dirname $0`

spatial schema generate -i "${ROOT}/../schema" -L ast_json -o "${ROOT}/../schema_json_ast"
spatial schema generate -i "${ROOT}/../schema" -L csharp -o "${ROOT}/Generated_Csharp_Worker"

"${ROOT}/../../bin/Release/GdkCodeGenerator.exe" --json-dir "${ROOT}/../schema_json_ast" --output-dir "${ROOT}/Generated"