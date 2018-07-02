// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Unity.Mathematics;

namespace Generated.Improbable.TestSchema.Blittable
{ 
    
    public struct SecondCommandResponse
    {
        public double Response;
    
        public static SecondCommandResponse ToNative(global::Improbable.TestSchema.Blittable.SecondCommandResponse spatialType)
        {
            var nativeType = new SecondCommandResponse();
            nativeType.Response = spatialType.response;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Blittable.SecondCommandResponse ToSpatial(global::Generated.Improbable.TestSchema.Blittable.SecondCommandResponse nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Blittable.SecondCommandResponse();
            spatialType.response = nativeType.Response;
            return spatialType;
        }
    }
}
