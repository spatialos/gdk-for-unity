// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable
{ 
    
    public struct WorkerRequirementSet
    {
        public global::System.Collections.Generic.List<global::Generated.Improbable.WorkerAttributeSet> AttributeSet;
    
        public WorkerRequirementSet(global::System.Collections.Generic.List<global::Generated.Improbable.WorkerAttributeSet> attributeSet)
        {
            AttributeSet = attributeSet;
        }
    
        public static class Serialization
        {
            public static void Serialize(WorkerRequirementSet instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    foreach (var value in instance.AttributeSet)
                    {
                        global::Generated.Improbable.WorkerAttributeSet.Serialization.Serialize(value, obj.AddObject(1));
                    }
                    
                }
            }
    
            public static WorkerRequirementSet Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new WorkerRequirementSet();
                {
                    var list = instance.AttributeSet = new global::System.Collections.Generic.List<global::Generated.Improbable.WorkerAttributeSet>();
                    var listLength = obj.GetObjectCount(1);
                    for (var i = 0; i < listLength; i++)
                    {
                        list.Add(global::Generated.Improbable.WorkerAttributeSet.Serialization.Deserialize(obj.IndexObject(1, (uint) i)));
                    }
                    
                }
                return instance;
            }
        }
    }
    
}
