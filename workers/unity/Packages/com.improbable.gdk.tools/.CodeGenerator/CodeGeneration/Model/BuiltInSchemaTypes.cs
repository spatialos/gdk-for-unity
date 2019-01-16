using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.Model
{
    public class BuiltInSchemaTypes
    {
        public const string BuiltInDouble = "double";
        public const string BuiltInFloat = "float";
        public const string BuiltInInt32 = "int32";
        public const string BuiltInInt64 = "int64";
        public const string BuiltInUint32 = "uint32";
        public const string BuiltInUint64 = "uint64";
        public const string BuiltInSint32 = "sint32";
        public const string BuiltInSint64 = "sint64";
        public const string BuiltInFixed32 = "fixed32";
        public const string BuiltInFixed64 = "fixed64";
        public const string BuiltInSfixed32 = "sfixed32";
        public const string BuiltInSfixed64 = "sfixed64";
        public const string BuiltInBool = "bool";
        public const string BuiltInString = "string";
        public const string BuiltInBytes = "bytes";
        public const string BuiltInEntityId = "EntityId";

        public static HashSet<string> BuiltInTypes = new HashSet<string>
        {
            BuiltInDouble,
            BuiltInFloat,
            BuiltInInt32,
            BuiltInInt64,
            BuiltInUint32,
            BuiltInUint64,
            BuiltInSint32,
            BuiltInSint64,
            BuiltInFixed32,
            BuiltInFixed64,
            BuiltInSfixed32,
            BuiltInSfixed64,
            BuiltInBool,
            BuiltInString,
            BuiltInBytes,
            BuiltInEntityId
        };
    }
}
