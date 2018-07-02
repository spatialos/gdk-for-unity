// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Unity.Mathematics;

namespace Generated.Improbable.TestSchema.Nonblittable
{ 
    
    public struct SecondCommandResponse
    {
        public string Response;
    
        public static SecondCommandResponse ToNative(global::Improbable.TestSchema.Nonblittable.SecondCommandResponse spatialType)
        {
            var nativeType = new SecondCommandResponse();
            nativeType.Response = spatialType.response;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Nonblittable.SecondCommandResponse ToSpatial(global::Generated.Improbable.TestSchema.Nonblittable.SecondCommandResponse nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Nonblittable.SecondCommandResponse();
            spatialType.response = nativeType.Response;
            return spatialType;
        }
    }
}
