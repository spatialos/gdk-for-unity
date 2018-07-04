// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using UnityEngine;
using Improbable.Gdk.Core;

namespace Generated.Improbable.TestSchema.Nonblittable
{ 
    public class SpatialOSNonBlittableComponent : Component, ISpatialComponentData
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

        private string stringField;

        public string StringField
        {
            get { return stringField; }
            set
            {
                DirtyBit = true;
                stringField = value;
            }
        }

        private global::System.Nullable<int> optionalField;

        public global::System.Nullable<int> OptionalField
        {
            get { return optionalField; }
            set
            {
                DirtyBit = true;
                optionalField = value;
            }
        }

        private global::System.Collections.Generic.List<int> listField;

        public global::System.Collections.Generic.List<int> ListField
        {
            get { return listField; }
            set
            {
                DirtyBit = true;
                listField = value;
            }
        }

        private global::System.Collections.Generic.Dictionary<int, string> mapField;

        public global::System.Collections.Generic.Dictionary<int, string> MapField
        {
            get { return mapField; }
            set
            {
                DirtyBit = true;
                mapField = value;
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<BlittableBool> BoolField;
            public Option<int> IntField;
            public Option<long> LongField;
            public Option<float> FloatField;
            public Option<double> DoubleField;
            public Option<string> StringField;
            public Option<global::System.Nullable<int>> OptionalField;
            public Option<global::System.Collections.Generic.List<int>> ListField;
            public Option<global::System.Collections.Generic.Dictionary<int, string>> MapField;
        }
    }
}
