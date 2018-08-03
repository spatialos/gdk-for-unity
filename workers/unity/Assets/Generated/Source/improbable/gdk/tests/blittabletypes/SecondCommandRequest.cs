// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{ 
    
    public struct SecondCommandRequest
    {
        public long Field;
    
        public static SecondCommandRequest ToNative(global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest spatialType)
        {
            var nativeType = new SecondCommandRequest();
            nativeType.Field = spatialType.field;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest ToSpatial(global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest();
            spatialType.field = nativeType.Field;
            return spatialType;
        }
    }
    
}
