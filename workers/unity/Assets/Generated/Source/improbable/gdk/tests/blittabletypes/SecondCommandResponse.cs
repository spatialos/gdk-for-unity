// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{ 
    
    public struct SecondCommandResponse
    {
        public double Response;
    
        public static SecondCommandResponse ToNative(global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse spatialType)
        {
            var nativeType = new SecondCommandResponse();
            nativeType.Response = spatialType.response;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse ToSpatial(global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse();
            spatialType.response = nativeType.Response;
            return spatialType;
        }
    }
    
}
