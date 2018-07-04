// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.TestSchema
{ 
    public struct SpatialOSNestedComponent : IComponentData, ISpatialComponentData
    {
        public BlittableBool DirtyBit { get; set; }

        private global::Generated.Improbable.TestSchema.TypeName nestedType;

        public global::Generated.Improbable.TestSchema.TypeName NestedType
        {
            get { return nestedType; }
            set
            {
                DirtyBit = true;
                nestedType = value;
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<global::Generated.Improbable.TestSchema.TypeName> NestedType;
        }
    }
}
