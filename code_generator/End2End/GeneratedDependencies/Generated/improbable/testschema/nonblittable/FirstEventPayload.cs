// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.TestSchema.Nonblittable
{ 
    
    public struct FirstEventPayload
    {
        public BlittableBool Field1;
        public string Field2;
    
        public static FirstEventPayload ToNative(global::Improbable.TestSchema.Nonblittable.FirstEventPayload spatialType)
        {
            var nativeType = new FirstEventPayload();
            nativeType.Field1 = spatialType.field1;
            nativeType.Field2 = spatialType.field2;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Nonblittable.FirstEventPayload ToSpatial(global::Generated.Improbable.TestSchema.Nonblittable.FirstEventPayload nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Nonblittable.FirstEventPayload();
            spatialType.field1 = nativeType.Field1;
            spatialType.field2 = nativeType.Field2;
            return spatialType;
        }
    }
    
}
