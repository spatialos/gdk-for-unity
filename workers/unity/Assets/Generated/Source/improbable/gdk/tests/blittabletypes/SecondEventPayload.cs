// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{ 
    
    public struct SecondEventPayload
    {
        public float Field1;
        public double Field2;
    
        public static SecondEventPayload ToNative(global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload spatialType)
        {
            var nativeType = new SecondEventPayload();
            nativeType.Field1 = spatialType.field1;
            nativeType.Field2 = spatialType.field2;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload ToSpatial(global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload();
            spatialType.field1 = nativeType.Field1;
            spatialType.field2 = nativeType.Field2;
            return spatialType;
        }
    }
    
}
