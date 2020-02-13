using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Improbable.Gdk.CodeGeneration.Model
{
    public enum PrimitiveType
    {
        Invalid = 0,
        Int32 = 1,
        Int64 = 2,
        Uint32 = 3,
        Uint64 = 4,
        Sint32 = 5,
        Sint64 = 6,
        Fixed32 = 7,
        Fixed64 = 8,
        Sfixed32 = 9,
        Sfixed64 = 10,
        Bool = 11,
        Float = 12,
        Double = 13,
        String = 14,
        EntityId = 15,
        Bytes = 16,
        Entity = 17
    }

    public static class PrimitiveTypeExtensions
    {
        public static bool IsBlittable(this PrimitiveType primitiveType)
        {
            return primitiveType != PrimitiveType.Bytes
                && primitiveType != PrimitiveType.String
                && primitiveType != PrimitiveType.Entity;
        }
    }

    public enum ValueType
    {
        Enum,
        Primitive,
        Type
    }

    public class TypeReference
    {
        public string Enum;
        public PrimitiveType Primitive;
        public string Type;

        public ValueType ValueTypeSelector
        {
            get
            {
                if (Primitive != PrimitiveType.Invalid)
                {
                    return ValueType.Primitive;
                }

                if (Type != null)
                {
                    return ValueType.Type;
                }

                if (Enum != null)
                {
                    return ValueType.Enum;
                }

                throw new InvalidOperationException("TypeReference doesn't have any type set.");
            }
        }
    }

    public class Value
    {
        public bool BoolValue;
        public string BytesValue;
        public double DoubleValue;
        public long EntityIdValue;
        public SchemaEnumValue EnumValue;
        public float FloatValue;
        public int Int32Value;
        public long Int64Value;
        public ListValueHolder ListValue;
        public MapValueHolder MapValue;
        public OptionValueHolder OptionValue;
        public SourceReference SourceReference;
        public string StringValue;
        public TypeValue TypeValue;
        public uint Uint32Value;
        public ulong Uint64Value;

        public class OptionValueHolder
        {
            public Value Value;
        }

        public class ListValueHolder
        {
            public IReadOnlyList<Value> Values;
        }

        public class MapValueHolder
        {
            public IReadOnlyList<MapPairValue> Values;

            public class MapPairValue
            {
                public Value Key;
                public Value Value;
            }
        }
    }

    public class SchemaEnumValue
    {
        public string Enum;
        public string EnumValue;

        public string Name;
        public string Value;
    }

    public class TypeValue
    {
        public IReadOnlyList<FieldValue> Fields;

        public string Type;

        public class FieldValue
        {
            public string Name;
            public SourceReference SourceReference;
            public Value Value;
        }
    }

    public class Annotation
    {
        public SourceReference SourceReference;
        public TypeValue TypeValue;
    }

    public abstract class Definition
    {
        public IReadOnlyList<Annotation> Annotations;
        public string Name;
        public SourceReference SourceReference;
    }

    public class EnumValueDefinition : Definition
    {
        public uint Value;
    }

    public enum FieldType
    {
        Option,
        List,
        Map,
        Singular
    }

    [DebuggerDisplay("{" + nameof(Name) + "}" + " ({" + nameof(FieldId) + "})")]
    public class FieldDefinition : Definition
    {
        public uint FieldId;

        public ListTypeRef ListType;
        public MapTypeRef MapType;
        public OptionTypeRef OptionType;
        public SingularTypeRef SingularType;

        public bool Transient;

        public FieldType TypeSelector
        {
            get
            {
                if (SingularType != null)
                {
                    return FieldType.Singular;
                }

                if (OptionType != null)
                {
                    return FieldType.Option;
                }

                if (ListType != null)
                {
                    return FieldType.List;
                }

                if (MapType != null)
                {
                    return FieldType.Map;
                }

                throw new InvalidOperationException("FieldType has no types set.");
            }
        }

        public class SingularTypeRef
        {
            public TypeReference Type;
        }

        public class OptionTypeRef
        {
            public TypeReference InnerType;
        }

        public class ListTypeRef
        {
            public TypeReference InnerType;
        }

        public class MapTypeRef
        {
            public TypeReference KeyType;
            public TypeReference ValueType;
        }
    }

    public abstract class QualifiedDefinition : Definition
    {
        public string QualifiedName;
    }

    [DebuggerDisplay("{" + nameof(QualifiedName) + "}")]
    public class EnumDefinition : QualifiedDefinition
    {
        public string OuterType;
        public IReadOnlyList<EnumValueDefinition> Values;
    }

    [DebuggerDisplay("{" + nameof(QualifiedName) + "}")]
    public class TypeDefinition : QualifiedDefinition
    {
        public string OuterType;
        public IReadOnlyList<FieldDefinition> Fields;
    }

    [DebuggerDisplay("{" + nameof(QualifiedName) + "} {" + nameof(ComponentId) + "}")]
    public class ComponentDefinition : QualifiedDefinition
    {
        public IReadOnlyList<CommandDefinition> Commands;
        public uint ComponentId;

        public string DataDefinition;
        public IReadOnlyList<EventDefinition> Events;
        public IReadOnlyList<FieldDefinition> Fields;

        [DebuggerDisplay("{" + nameof(QualifiedName) + "}")]
        public class EventDefinition : Definition
        {
            public uint EventIndex;
            public string Type;
        }

        [DebuggerDisplay("{" + nameof(QualifiedName) + "}" + " {" + nameof(CommandIndex) + "}")]
        public class CommandDefinition : Definition
        {
            public uint CommandIndex;
            public string RequestType;
            public string ResponseType;
        }
    }

    public class SourceReference
    {
        public uint Column;
        public uint Line;
    }

    public class Package
    {
        public string Name;
        public SourceReference SourceReference;
    }

    public class Import
    {
        public string Path;
        public SourceReference SourceReference;
    }

    public class SchemaFile
    {
        public string CanonicalPath;
        public IReadOnlyList<ComponentDefinition> Components;
        public IReadOnlyList<EnumDefinition> Enums;
        public IReadOnlyList<Import> Imports;
        public Package Package;
        public IReadOnlyList<TypeDefinition> Types;
    }

    public class SchemaBundle
    {
        public IReadOnlyList<SchemaFile> SchemaFiles;

        public static SchemaBundle LoadBundle(string json)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new PascalCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                MissingMemberHandling = MissingMemberHandling.Error
            };

            return JsonConvert.DeserializeObject<SchemaBundle>(json, settings);
        }

        private class PascalCaseNamingStrategy : NamingStrategy
        {
            protected override string ResolvePropertyName(string name)
            {
                var pascal = char.ToLowerInvariant(name[0]) + name.Substring(1, name.Length - 1);
                return pascal;
            }
        }
    }
}
