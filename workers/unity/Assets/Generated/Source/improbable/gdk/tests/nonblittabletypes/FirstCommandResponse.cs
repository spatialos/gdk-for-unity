// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{ 
    
    public struct FirstCommandResponse
    {
        public string Response;
    
        public static FirstCommandResponse ToNative(global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse spatialType)
        {
            var nativeType = new FirstCommandResponse();
            nativeType.Response = spatialType.response;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse ToSpatial(global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse();
            spatialType.response = nativeType.Response;
            return spatialType;
        }
    }
    
}
