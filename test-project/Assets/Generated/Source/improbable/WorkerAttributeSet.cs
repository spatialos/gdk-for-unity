// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable
{ 
    
    public struct WorkerAttributeSet
    {
        public global::System.Collections.Generic.List<string> Attribute;
    
        public WorkerAttributeSet(global::System.Collections.Generic.List<string> attribute)
        {
            Attribute = attribute;
        }
    
        public static class Serialization
        {
            public static void Serialize(WorkerAttributeSet instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    foreach (var value in instance.Attribute)
                    {
                        obj.AddString(1, value);
                    }
                    
                }
            }
    
            public static WorkerAttributeSet Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new WorkerAttributeSet();
                {
                    var list = instance.Attribute = new global::System.Collections.Generic.List<string>();
                    var listLength = obj.GetStringCount(1);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(obj.IndexString(1, (uint) i));
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
