using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public static class SchemaFunctionMappings
    {
        private static readonly HashSet<string> BuiltInTypeWithSchemaFunctions = new HashSet<string>
        {
            BuiltInSchemaTypes.BuiltInBool,
            BuiltInSchemaTypes.BuiltInBytes,
            BuiltInSchemaTypes.BuiltInDouble,
            BuiltInSchemaTypes.BuiltInEntityId,
            BuiltInSchemaTypes.BuiltInFixed32,
            BuiltInSchemaTypes.BuiltInFixed64,
            BuiltInSchemaTypes.BuiltInFloat,
            BuiltInSchemaTypes.BuiltInInt32,
            BuiltInSchemaTypes.BuiltInInt64,
            BuiltInSchemaTypes.BuiltInSfixed32,
            BuiltInSchemaTypes.BuiltInSfixed64,
            BuiltInSchemaTypes.BuiltInUint32,
            BuiltInSchemaTypes.BuiltInUint64,
            BuiltInSchemaTypes.BuiltInSint32,
            BuiltInSchemaTypes.BuiltInSint64,
            BuiltInSchemaTypes.BuiltInString,
        };

        public static string AddSchemaFunctionFromType(string builtInType)
        {
            if (!BuiltInTypeWithSchemaFunctions.Contains(builtInType))
            {
                throw new ArgumentException($"There are no raw schema functions defined for {builtInType}");
            }

            return $"Add{ToUppercase(builtInType)}";
        }

        public static string GetSchemaFunctionFromType(string builtInType)
        {
            if (!BuiltInTypeWithSchemaFunctions.Contains(builtInType))
            {
                throw new ArgumentException($"There are no raw schema functions defined for {builtInType}");
            }

            return builtInType == BuiltInSchemaTypes.BuiltInEntityId
                ? $"Get{ToUppercase(builtInType)}Struct"
                : $"Get{ToUppercase(builtInType)}";
        }

        public static string GetCountSchemaFunctionFromType(string builtInType)
        {
            if (!BuiltInTypeWithSchemaFunctions.Contains(builtInType))
            {
                throw new ArgumentException($"There are no raw schema functions defined for {builtInType}");
            }

            return $"Get{ToUppercase(builtInType)}Count";
        }

        public static string IndexSchemaFunctionFromType(string builtInType)
        {
            if (!BuiltInTypeWithSchemaFunctions.Contains(builtInType))
            {
                throw new ArgumentException($"There are no raw schema functions defined for {builtInType}");
            }

            return builtInType == BuiltInSchemaTypes.BuiltInEntityId
                ? $"Index{ToUppercase(builtInType)}Struct"
                : $"Index{ToUppercase(builtInType)}";
        }

        private static string ToUppercase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s.First().ToString().ToUpper() + s.Substring(1);
        }
    }
}
