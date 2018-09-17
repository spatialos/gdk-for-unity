// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{ 
    
    public struct Empty
    {
    
        public static class Serialization
        {
            public static void Serialize(Empty instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
            }
    
            public static Empty Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new Empty();
                return instance;
            }
        }
    }
    
}
