using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.Model
{
    public class BuiltInSchemaTypes
    {
        public const string BuiltInDouble = "Double";
        public const string BuiltInFloat = "Float";
        public const string BuiltInInt32 = "Int32";
        public const string BuiltInInt64 = "Int64";
        public const string BuiltInUint32 = "Uint32";
        public const string BuiltInUint64 = "Uint64";
        public const string BuiltInSint32 = "Sint32";
        public const string BuiltInSint64 = "Sint64";
        public const string BuiltInFixed32 = "Fixed32";
        public const string BuiltInFixed64 = "Fixed64";
        public const string BuiltInSfixed32 = "Sfixed32";
        public const string BuiltInSfixed64 = "Sfixed64";
        public const string BuiltInBool = "Bool";
        public const string BuiltInString = "String";
        public const string BuiltInBytes = "Bytes";
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
