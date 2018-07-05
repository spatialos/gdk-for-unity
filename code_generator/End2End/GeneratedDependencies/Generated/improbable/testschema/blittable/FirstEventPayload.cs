// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.TestSchema.Blittable
{ 
    
    public struct FirstEventPayload
    {
        public BlittableBool Field1;
        public int Field2;
    
        public static FirstEventPayload ToNative(global::Improbable.TestSchema.Blittable.FirstEventPayload spatialType)
        {
            var nativeType = new FirstEventPayload();
            nativeType.Field1 = spatialType.field1;
            nativeType.Field2 = spatialType.field2;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Blittable.FirstEventPayload ToSpatial(global::Generated.Improbable.TestSchema.Blittable.FirstEventPayload nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Blittable.FirstEventPayload();
            spatialType.field1 = nativeType.Field1;
            spatialType.field2 = nativeType.Field2;
            return spatialType;
        }
    }
    
}
