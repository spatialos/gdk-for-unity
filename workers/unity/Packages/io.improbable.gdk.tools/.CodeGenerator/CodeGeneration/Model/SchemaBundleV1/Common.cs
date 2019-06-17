using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1
{
    public class Identifier : IEquatable<Identifier>
    {
        [JsonProperty("qualifiedName")] public string QualifiedName;
        [JsonProperty("name")] public string Name;
        [JsonProperty("path")] public List<string> Path;

        public string PackagePath => string.Join(".", Path.Take(Path.Count - 1));

        public bool Equals(Identifier other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(QualifiedName, other.QualifiedName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return QualifiedName != null ? QualifiedName.GetHashCode() : 0;
        }
    }

    public class FullyQualifiedReference
    {
        [JsonProperty("qualifiedName")] public string QualifiedName;
    }

    public class Field
    {
        [JsonProperty("identifier")] public Identifier Identifier;
        [JsonProperty("fieldId")] public uint FieldId;
        [JsonProperty("transient")] public bool IsTransient;
        [JsonProperty("listType")] public ListType List;
        [JsonProperty("mapType")] public MapType Map;
        [JsonProperty("optionType")] public OptionType Option;
        [JsonProperty("singularType")] public SingularType Singular;
        [JsonProperty("annotations")] public List<AnnotationRaw> Annotations;

        public class ListType
        {
            [JsonProperty("innerType")] public InnerType InnerType;
        }

        public class MapType
        {
            [JsonProperty("keyType")] public InnerType KeyType;
            [JsonProperty("valueType")] public InnerType ValueType;
        }

        public class OptionType
        {
            [JsonProperty("innerType")] public InnerType InnerType;
        }

        public class SingularType
        {
            [JsonProperty("type")] public InnerType Type;
        }

        // Note: Only one of these fields should ever be non-null.
        // The type is either a primitive or its a user defined type.
        public class InnerType
        {
            [JsonProperty("type")] public FullyQualifiedReference UserType;
            [JsonProperty("primitive")] public string Primitive;
            [JsonProperty("enum")] public FullyQualifiedReference EnumType;
        }
    }
}
