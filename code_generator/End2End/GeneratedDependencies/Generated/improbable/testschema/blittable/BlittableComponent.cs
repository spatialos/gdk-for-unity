// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Mathematics;
using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.TestSchema.Blittable
{ 
    public struct SpatialOSBlittableComponent : IComponentData, ISpatialComponentData
    {
        public bool1 DirtyBit { get; set; }

        private bool1 boolField;

        public bool1 BoolField
        {
            get { return boolField; }
            set
            {
                DirtyBit = true;
                boolField = value;
            }
        }

        private int intField;

        public int IntField
        {
            get { return intField; }
            set
            {
                DirtyBit = true;
                intField = value;
            }
        }

        private long longField;

        public long LongField
        {
            get { return longField; }
            set
            {
                DirtyBit = true;
                longField = value;
            }
        }

        private float floatField;

        public float FloatField
        {
            get { return floatField; }
            set
            {
                DirtyBit = true;
                floatField = value;
            }
        }

        private double doubleField;

        public double DoubleField
        {
            get { return doubleField; }
            set
            {
                DirtyBit = true;
                doubleField = value;
            }
        }
    }
}
