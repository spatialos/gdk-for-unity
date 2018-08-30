// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable
{ 
    
    public struct Vector3f
    {
        public float X;
        public float Y;
        public float Z;
    
        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    
        public static class Serialization
        {
            public static void Serialize(Vector3f instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    obj.AddFloat(1, instance.X);
                }
                {
                    obj.AddFloat(2, instance.Y);
                }
                {
                    obj.AddFloat(3, instance.Z);
                }
            }
    
            public static Vector3f Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new Vector3f();
                {
                    instance.X = obj.GetFloat(1);
                }
                {
                    instance.Y = obj.GetFloat(2);
                }
                {
                    instance.Z = obj.GetFloat(3);
                }
                return instance;
            }
        }
    }
    
}
