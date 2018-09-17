// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.NonblittableTypes
{ 
    
    public struct FirstCommandResponse
    {
        public string Response;
    
        public FirstCommandResponse(string response)
        {
            Response = response;
        }
    
        public static class Serialization
        {
            public static void Serialize(FirstCommandResponse instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    obj.AddString(1, instance.Response);
                }
            }
    
            public static FirstCommandResponse Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new FirstCommandResponse();
                {
                    instance.Response = obj.GetString(1);
                }
                return instance;
            }
        }
    }
    
}
