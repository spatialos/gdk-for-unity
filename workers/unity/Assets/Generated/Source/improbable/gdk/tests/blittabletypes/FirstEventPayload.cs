// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{ 
    
    public struct FirstEventPayload
    {
        public BlittableBool Field1;
        public int Field2;
    
        public static FirstEventPayload ToNative(global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload spatialType)
        {
            var nativeType = new FirstEventPayload();
            nativeType.Field1 = spatialType.field1;
            nativeType.Field2 = spatialType.field2;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload ToSpatial(global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload();
            spatialType.field1 = nativeType.Field1;
            spatialType.field2 = nativeType.Field2;
            return spatialType;
        }
    }
    
}
