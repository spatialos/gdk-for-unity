// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{ 
    
    public struct FirstCommandResponse
    {
        public BlittableBool Response;
    
        public static FirstCommandResponse ToNative(global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse spatialType)
        {
            var nativeType = new FirstCommandResponse();
            nativeType.Response = spatialType.response;
            return nativeType;
        }
    
        public static global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse ToSpatial(global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse nativeType)
        {
            var spatialType = new global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse();
            spatialType.response = nativeType.Response;
            return spatialType;
        }
    }
    
}
