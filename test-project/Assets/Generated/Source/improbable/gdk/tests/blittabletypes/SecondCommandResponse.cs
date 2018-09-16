// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.BlittableTypes
{ 
    
    public struct SecondCommandResponse
    {
        public double Response;
    
        public SecondCommandResponse(double response)
        {
            Response = response;
        }
    
        public static class Serialization
        {
            public static void Serialize(SecondCommandResponse instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    obj.AddDouble(1, instance.Response);
                }
            }
    
            public static SecondCommandResponse Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new SecondCommandResponse();
                {
                    instance.Response = obj.GetDouble(1);
                }
                return instance;
            }
        }
    }
    
}
