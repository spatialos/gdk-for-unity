// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{ 
    
    public struct FirstCommandRequest
    {
        public int Field;
    
        public static FirstCommandRequest ToNative(global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest spatialType)
        {
            var nativeType = new FirstCommandRequest();
            nativeType.Field = spatialType.field;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest ToSpatial(global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest();
            spatialType.field = nativeType.Field;
            return spatialType;
        }
    }
    
}
