// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Linq;
using Improbable.Gdk.Core;

namespace Generated.Improbable
{ 
    
    public struct Vector3d
    {
        public double X;
        public double Y;
        public double Z;
    
        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    
        public static class Serialization
        {
            public static void Serialize(Vector3d instance, global::Improbable.Worker.Core.SchemaObject obj)
            {
                {
                    obj.AddDouble(1, instance.X);
                }
                {
                    obj.AddDouble(2, instance.Y);
                }
                {
                    obj.AddDouble(3, instance.Z);
                }
            }
    
            public static Vector3d Deserialize(global::Improbable.Worker.Core.SchemaObject obj)
            {
                var instance = new Vector3d();
                {
                    instance.X = obj.GetDouble(1);
                }
                {
                    instance.Y = obj.GetDouble(2);
                }
                {
                    instance.Z = obj.GetDouble(3);
                }
                return instance;
            }
        }
    }
    
}
