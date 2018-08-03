// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{ 
    
    public struct SecondCommandRequest
    {
        public long Field;
    
        public static SecondCommandRequest ToNative(global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest spatialType)
        {
            var nativeType = new SecondCommandRequest();
            nativeType.Field = spatialType.field;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest ToSpatial(global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest();
            spatialType.field = nativeType.Field;
            return spatialType;
        }
    }
    
}
