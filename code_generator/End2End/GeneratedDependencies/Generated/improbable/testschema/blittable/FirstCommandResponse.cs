// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Unity.Mathematics;

namespace Generated.Improbable.TestSchema.Blittable
{ 
    
    public struct FirstCommandResponse
    {
        public bool1 Response;
    
        public static FirstCommandResponse ToNative(global::Improbable.TestSchema.Blittable.FirstCommandResponse spatialType)
        {
            var nativeType = new FirstCommandResponse();
            nativeType.Response = spatialType.response;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Blittable.FirstCommandResponse ToSpatial(global::Generated.Improbable.TestSchema.Blittable.FirstCommandResponse nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Blittable.FirstCommandResponse();
            spatialType.response = nativeType.Response;
            return spatialType;
        }
    }
}
