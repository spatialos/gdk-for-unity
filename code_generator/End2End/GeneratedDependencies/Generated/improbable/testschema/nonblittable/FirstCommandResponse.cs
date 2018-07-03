// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.TestSchema.Nonblittable
{ 
    
    public struct FirstCommandResponse
    {
        public string Response;
    
        public static FirstCommandResponse ToNative(global::Improbable.TestSchema.Nonblittable.FirstCommandResponse spatialType)
        {
            var nativeType = new FirstCommandResponse();
            nativeType.Response = spatialType.response;
            return nativeType;
        }
    
        public static global::Improbable.TestSchema.Nonblittable.FirstCommandResponse ToSpatial(global::Generated.Improbable.TestSchema.Nonblittable.FirstCommandResponse nativeType)
        {
            var spatialType = new global::Improbable.TestSchema.Nonblittable.FirstCommandResponse();
            spatialType.response = nativeType.Response;
            return spatialType;
        }
    }
    
}
