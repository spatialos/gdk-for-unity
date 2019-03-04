// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    
    [System.Serializable]
    public struct SecondCommandResponse
    {
        public string Response;
    
        public SecondCommandResponse(string response)
        {
            Response = response;
        }
        public static class Serialization
        {
            public static void Serialize(SecondCommandResponse instance, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                {
                    obj.AddString(1, instance.Response);
                }
            }
    
            public static SecondCommandResponse Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var instance = new SecondCommandResponse();
                {
                    instance.Response = obj.GetString(1);
                }
                return instance;
            }
        }
    }
    
}
