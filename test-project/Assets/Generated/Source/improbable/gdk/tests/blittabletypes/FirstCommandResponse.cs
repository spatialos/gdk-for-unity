// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.BlittableTypes
{ 
    
    public struct FirstCommandResponse
    {
        public BlittableBool Response;
    
        public FirstCommandResponse(BlittableBool response)
        {
            Response = response;
        }
    
        public static class Serialization
        {
            public static void Serialize(FirstCommandResponse instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    obj.AddBool(1, instance.Response);
                }
            }
    
            public static FirstCommandResponse Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new FirstCommandResponse();
                {
                    instance.Response = obj.GetBool(1);
                }
                return instance;
            }
        }
    }
    
}
