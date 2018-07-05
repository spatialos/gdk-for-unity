// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.TestSchema.Nonblittable
{ 
    
    public struct SecondCommandRequest
    {
        public long Field;
    
        public static SecondCommandRequest ToNative(global::Improbable.TestSchema.Nonblittable.SecondCommandRequest spatialType)
        {
            var nativeType = new SecondCommandRequest();
            nativeType.Field = spatialType.field;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Nonblittable.SecondCommandRequest ToSpatial(global::Generated.Improbable.TestSchema.Nonblittable.SecondCommandRequest nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Nonblittable.SecondCommandRequest();
            spatialType.field = nativeType.Field;
            return spatialType;
        }
    }
    
}
