// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{ 
    public struct SpatialOSBlittableComponent : IComponentData, ISpatialComponentData
    {
        public BlittableBool DirtyBit { get; set; }

        private BlittableBool boolField;

        public BlittableBool BoolField
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

        public struct Update : ISpatialComponentUpdate
        {
            public Option<BlittableBool> BoolField;
            public Option<int> IntField;
            public Option<long> LongField;
            public Option<float> FloatField;
            public Option<double> DoubleField;
        }
    }
}
