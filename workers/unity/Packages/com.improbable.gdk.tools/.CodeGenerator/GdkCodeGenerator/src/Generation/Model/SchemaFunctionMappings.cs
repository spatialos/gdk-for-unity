using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public class SchemaFunctionMappings
    {
        public static HashSet<string> BuiltInTypeWithSchemaFunctions = new HashSet<string>
        {
            BuiltInTypeConstants.builtInBool,
            BuiltInTypeConstants.builtInBytes,
            BuiltInTypeConstants.builtInDouble,
            BuiltInTypeConstants.builtInEntityId,
            BuiltInTypeConstants.builtInFixed32,
            BuiltInTypeConstants.builtInFixed64,
            BuiltInTypeConstants.builtInFloat,
            BuiltInTypeConstants.builtInInt32,
            BuiltInTypeConstants.builtInInt64,
            BuiltInTypeConstants.builtInSfixed32,
            BuiltInTypeConstants.builtInSfixed64,
            BuiltInTypeConstants.builtInUint32,
            BuiltInTypeConstants.builtInUint64,
            BuiltInTypeConstants.builtInSint32,
            BuiltInTypeConstants.builtInSint64,
            BuiltInTypeConstants.builtInString,
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

            return $"Get{ToUppercase(builtInType)}";
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

            return $"Index{ToUppercase(builtInType)}";
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
