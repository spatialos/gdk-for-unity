// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Unity.Mathematics;

namespace Generated.Improbable.TestSchema.Nonblittable
{ 
    
    public struct SecondEventPayload
    {
        public float Field1;
        public global::System.Collections.Generic.List<double> Field2;
    
        public static SecondEventPayload ToNative(global::Improbable.TestSchema.Nonblittable.SecondEventPayload spatialType)
        {
            var nativeType = new SecondEventPayload();
            nativeType.Field1 = spatialType.field1;
            nativeType.Field2 = spatialType.field2;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Nonblittable.SecondEventPayload ToSpatial(global::Generated.Improbable.TestSchema.Nonblittable.SecondEventPayload nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Nonblittable.SecondEventPayload();
            spatialType.field1 = nativeType.Field1;
            spatialType.field2 = new global::Improbable.Collections.List<double>(nativeType.Field2);
            return spatialType;
        }
    }
}
