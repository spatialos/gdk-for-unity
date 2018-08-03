// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{ 
    
    public struct FirstEventPayload
    {
        public BlittableBool Field1;
        public string Field2;
    
        public static FirstEventPayload ToNative(global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload spatialType)
        {
            var nativeType = new FirstEventPayload();
            nativeType.Field1 = spatialType.field1;
            nativeType.Field2 = spatialType.field2;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload ToSpatial(global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload();
            spatialType.field1 = nativeType.Field1;
            spatialType.field2 = nativeType.Field2;
            return spatialType;
        }
    }
    
}
