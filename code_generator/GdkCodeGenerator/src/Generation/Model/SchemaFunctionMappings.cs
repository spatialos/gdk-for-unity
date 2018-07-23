using System;
using System.Collections.Generic;
using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public class SchemaFunctionMappings
    {
        public static Dictionary<string, string> PrimitiveTypeToAddSchemaFunction = new Dictionary<string, string>
        {
            { BuiltInTypeConstants.builtInBool, "AddBool"},
            { BuiltInTypeConstants.builtInBytes, "AddBytes" },
            { BuiltInTypeConstants.builtInDouble, "AddDouble" },
            { BuiltInTypeConstants.builtInEntityId, "AddEntityId" },
            { BuiltInTypeConstants.builtInFixed32, "AddFixed32" },
            { BuiltInTypeConstants.builtInFixed64, "AddFixed64" },
            { BuiltInTypeConstants.builtInFloat, "AddFloat" },
            { BuiltInTypeConstants.builtInInt32, "AddInt32" },
            { BuiltInTypeConstants.builtInInt64, "AddInt64" },
            { BuiltInTypeConstants.builtInSfixed32, "AddSfixed32" },
            { BuiltInTypeConstants.builtInSfixed64, "AddSfixed64" },
            { BuiltInTypeConstants.builtInUint32, "AddUint32" },
            { BuiltInTypeConstants.builtInUint64, "AddUint64" },
            { BuiltInTypeConstants.builtInSint32, "AddSint32" },
            { BuiltInTypeConstants.builtInSint64, "AddSint64" },
            { BuiltInTypeConstants.builtInString, "AddString" }
        };

        public static Dictionary<string, string> PrimitiveTypeToGetSchemaFunction = new Dictionary<string, string>
        {
            { BuiltInTypeConstants.builtInBool, "GetBool"},
            { BuiltInTypeConstants.builtInBytes, "GetBytes" },
            { BuiltInTypeConstants.builtInDouble, "GetDouble" },
            { BuiltInTypeConstants.builtInEntityId, "GetEntityId" },
            { BuiltInTypeConstants.builtInFixed32, "GetFixed32" },
            { BuiltInTypeConstants.builtInFixed64, "GetFixed64" },
            { BuiltInTypeConstants.builtInFloat, "GetFloat" },
            { BuiltInTypeConstants.builtInInt32, "GetInt32" },
            { BuiltInTypeConstants.builtInInt64, "GetInt64" },
            { BuiltInTypeConstants.builtInSfixed32, "GetSfixed32" },
            { BuiltInTypeConstants.builtInSfixed64, "GetSfixed64" },
            { BuiltInTypeConstants.builtInUint32, "GetUint32" },
            { BuiltInTypeConstants.builtInUint64, "GetUint64" },
            { BuiltInTypeConstants.builtInSint32, "GetSint32" },
            { BuiltInTypeConstants.builtInSint64, "GetSint64" },
            { BuiltInTypeConstants.builtInString, "GetString" }
        };

        public static Dictionary<string, string> PrimitiveTypeToGetCountSchemaFunction = new Dictionary<string, string>
        {
            { BuiltInTypeConstants.builtInBool, "GetBoolCount"},
            { BuiltInTypeConstants.builtInBytes, "GetBytesCount" },
            { BuiltInTypeConstants.builtInDouble, "GetDoubleCount" },
            { BuiltInTypeConstants.builtInEntityId, "GetEntityIdCount" },
            { BuiltInTypeConstants.builtInFixed32, "GetFixed32Count" },
            { BuiltInTypeConstants.builtInFixed64, "GetFixed64Count" },
            { BuiltInTypeConstants.builtInFloat, "GetFloatCount" },
            { BuiltInTypeConstants.builtInInt32, "GetInt32Count" },
            { BuiltInTypeConstants.builtInInt64, "GetInt64Count" },
            { BuiltInTypeConstants.builtInSfixed32, "GetSfixed32Count" },
            { BuiltInTypeConstants.builtInSfixed64, "GetSfixed64Count" },
            { BuiltInTypeConstants.builtInUint32, "GetUint32Count" },
            { BuiltInTypeConstants.builtInUint64, "GetUint64Count" },
            { BuiltInTypeConstants.builtInSint32, "GetSint32Count" },
            { BuiltInTypeConstants.builtInSint64, "GetSint64Count" },
            { BuiltInTypeConstants.builtInString, "GetStringCount" }
        };
    }
}
