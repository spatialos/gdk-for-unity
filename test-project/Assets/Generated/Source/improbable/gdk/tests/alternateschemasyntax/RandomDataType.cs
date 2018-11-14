// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.AlternateSchemaSyntax
{ 
    
    public struct RandomDataType
    {
        public int Value;
    
        public RandomDataType(int value)
        {
            Value = value;
        }
    
        public static class Serialization
        {
            public static void Serialize(RandomDataType instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    obj.AddInt32(1, instance.Value);
                }
            }
    
            public static RandomDataType Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new RandomDataType();
                {
                    instance.Value = obj.GetInt32(1);
                }
                return instance;
            }
        }
    }
    
}
