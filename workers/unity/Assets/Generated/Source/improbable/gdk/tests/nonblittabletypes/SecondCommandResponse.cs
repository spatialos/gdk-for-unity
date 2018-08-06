// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{ 
    
    public struct SecondCommandResponse
    {
        public string Response;
    
        public static SecondCommandResponse ToNative(global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse spatialType)
        {
            var nativeType = new SecondCommandResponse();
            nativeType.Response = spatialType.response;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse ToSpatial(global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse();
            spatialType.response = nativeType.Response;
            return spatialType;
        }
    }
    
}
