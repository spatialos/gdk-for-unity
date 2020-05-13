// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Entities;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveMapKey
    {
        public const uint ComponentId = 197719;

        public unsafe struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>
        {
            // Bit masks for tracking which component properties were changed locally and need to be synced.
            private fixed UInt32 dirtyBits[1];

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<bool, string>>.ReferenceHandle field1Handle;

            public global::System.Collections.Generic.Dictionary<bool, string> Field1
            {
                get => field1Handle.Get();
                set
                {
                    MarkDataDirty(0);
                    field1Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<float, string>>.ReferenceHandle field2Handle;

            public global::System.Collections.Generic.Dictionary<float, string> Field2
            {
                get => field2Handle.Get();
                set
                {
                    MarkDataDirty(1);
                    field2Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<byte[], string>>.ReferenceHandle field3Handle;

            public global::System.Collections.Generic.Dictionary<byte[], string> Field3
            {
                get => field3Handle.Get();
                set
                {
                    MarkDataDirty(2);
                    field3Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<int, string>>.ReferenceHandle field4Handle;

            public global::System.Collections.Generic.Dictionary<int, string> Field4
            {
                get => field4Handle.Get();
                set
                {
                    MarkDataDirty(3);
                    field4Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<long, string>>.ReferenceHandle field5Handle;

            public global::System.Collections.Generic.Dictionary<long, string> Field5
            {
                get => field5Handle.Get();
                set
                {
                    MarkDataDirty(4);
                    field5Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<double, string>>.ReferenceHandle field6Handle;

            public global::System.Collections.Generic.Dictionary<double, string> Field6
            {
                get => field6Handle.Get();
                set
                {
                    MarkDataDirty(5);
                    field6Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<string, string>>.ReferenceHandle field7Handle;

            public global::System.Collections.Generic.Dictionary<string, string> Field7
            {
                get => field7Handle.Get();
                set
                {
                    MarkDataDirty(6);
                    field7Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<uint, string>>.ReferenceHandle field8Handle;

            public global::System.Collections.Generic.Dictionary<uint, string> Field8
            {
                get => field8Handle.Get();
                set
                {
                    MarkDataDirty(7);
                    field8Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<ulong, string>>.ReferenceHandle field9Handle;

            public global::System.Collections.Generic.Dictionary<ulong, string> Field9
            {
                get => field9Handle.Get();
                set
                {
                    MarkDataDirty(8);
                    field9Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<int, string>>.ReferenceHandle field10Handle;

            public global::System.Collections.Generic.Dictionary<int, string> Field10
            {
                get => field10Handle.Get();
                set
                {
                    MarkDataDirty(9);
                    field10Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<long, string>>.ReferenceHandle field11Handle;

            public global::System.Collections.Generic.Dictionary<long, string> Field11
            {
                get => field11Handle.Get();
                set
                {
                    MarkDataDirty(10);
                    field11Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<uint, string>>.ReferenceHandle field12Handle;

            public global::System.Collections.Generic.Dictionary<uint, string> Field12
            {
                get => field12Handle.Get();
                set
                {
                    MarkDataDirty(11);
                    field12Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<ulong, string>>.ReferenceHandle field13Handle;

            public global::System.Collections.Generic.Dictionary<ulong, string> Field13
            {
                get => field13Handle.Get();
                set
                {
                    MarkDataDirty(12);
                    field13Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<int, string>>.ReferenceHandle field14Handle;

            public global::System.Collections.Generic.Dictionary<int, string> Field14
            {
                get => field14Handle.Get();
                set
                {
                    MarkDataDirty(13);
                    field14Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<long, string>>.ReferenceHandle field15Handle;

            public global::System.Collections.Generic.Dictionary<long, string> Field15
            {
                get => field15Handle.Get();
                set
                {
                    MarkDataDirty(14);
                    field15Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>>.ReferenceHandle field16Handle;

            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string> Field16
            {
                get => field16Handle.Get();
                set
                {
                    MarkDataDirty(15);
                    field16Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>>.ReferenceHandle field17Handle;

            public global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string> Field17
            {
                get => field17Handle.Get();
                set
                {
                    MarkDataDirty(16);
                    field17Handle.Set(value);
                }
            }

            internal global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>>.ReferenceHandle field18Handle;

            public global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string> Field18
            {
                get => field18Handle.Get();
                set
                {
                    MarkDataDirty(17);
                    field18Handle.Set(value);
                }
            }

            public bool IsDataDirty()
            {
                var isDataDirty = false;

                isDataDirty |= (dirtyBits[0] != 0x0);

                return isDataDirty;
            }

            /*
            The propertyIndex argument counts up from 0 in the order defined in your schema component.
            It is not the schema field number itself. For example:
            component MyComponent
            {
                id = 1337;
                bool val_a = 1;
                bool val_b = 3;
            }
            In that case, val_a corresponds to propertyIndex 0 and val_b corresponds to propertyIndex 1 in this method.
            This method throws an InvalidOperationException in case your component doesn't contain properties.
            */

            public bool IsDataDirty(int propertyIndex)
            {
                ValidateFieldIndex(propertyIndex);

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex >> 4;
                return (dirtyBits[dirtyBitsByteIndex] & (0x1 << (propertyIndex & 31))) != 0x0;
            }

            // Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
            // This method throws an InvalidOperationException in case your component doesn't contain properties.
            public void MarkDataDirty(int propertyIndex)
            {
                ValidateFieldIndex(propertyIndex);

                // Retrieve the dirtyBits[0-n] field that tracks this property.
                var dirtyBitsByteIndex = propertyIndex >> 4;
                dirtyBits[dirtyBitsByteIndex] |= (UInt32) (0x1 << (propertyIndex & 31));
            }

            public void MarkDataClean()
            {
                dirtyBits[0] = 0x0;
            }

            [Conditional("DEBUG")]
            private void ValidateFieldIndex(int propertyIndex)
            {
                if (propertyIndex < 0 || propertyIndex >= 18)
                {
                    throw new ArgumentException("\"propertyIndex\" argument out of range. Valid range is [0, 17]. " +
                        "Unless you are using custom component replication code, this is most likely caused by a code generation bug. " +
                        "Please contact SpatialOS support if you encounter this issue.");
                }
            }

            public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
            {
                var componentDataSchema = new ComponentData(197719, SchemaComponentData.Create());
                Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
                var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

                componentDataSchema.SchemaData?.Destroy();
                componentDataSchema.SchemaData = null;

                return snapshot;
            }
        }

        public struct HasAuthority : IComponentData
        {
        }

        [global::System.Serializable]
        public struct Snapshot : ISpatialComponentSnapshot
        {
            public uint ComponentId => 197719;

            public global::System.Collections.Generic.Dictionary<bool, string> Field1;
            public global::System.Collections.Generic.Dictionary<float, string> Field2;
            public global::System.Collections.Generic.Dictionary<byte[], string> Field3;
            public global::System.Collections.Generic.Dictionary<int, string> Field4;
            public global::System.Collections.Generic.Dictionary<long, string> Field5;
            public global::System.Collections.Generic.Dictionary<double, string> Field6;
            public global::System.Collections.Generic.Dictionary<string, string> Field7;
            public global::System.Collections.Generic.Dictionary<uint, string> Field8;
            public global::System.Collections.Generic.Dictionary<ulong, string> Field9;
            public global::System.Collections.Generic.Dictionary<int, string> Field10;
            public global::System.Collections.Generic.Dictionary<long, string> Field11;
            public global::System.Collections.Generic.Dictionary<uint, string> Field12;
            public global::System.Collections.Generic.Dictionary<ulong, string> Field13;
            public global::System.Collections.Generic.Dictionary<int, string> Field14;
            public global::System.Collections.Generic.Dictionary<long, string> Field15;
            public global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string> Field16;
            public global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string> Field17;
            public global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string> Field18;

            public Snapshot(global::System.Collections.Generic.Dictionary<bool, string> field1, global::System.Collections.Generic.Dictionary<float, string> field2, global::System.Collections.Generic.Dictionary<byte[], string> field3, global::System.Collections.Generic.Dictionary<int, string> field4, global::System.Collections.Generic.Dictionary<long, string> field5, global::System.Collections.Generic.Dictionary<double, string> field6, global::System.Collections.Generic.Dictionary<string, string> field7, global::System.Collections.Generic.Dictionary<uint, string> field8, global::System.Collections.Generic.Dictionary<ulong, string> field9, global::System.Collections.Generic.Dictionary<int, string> field10, global::System.Collections.Generic.Dictionary<long, string> field11, global::System.Collections.Generic.Dictionary<uint, string> field12, global::System.Collections.Generic.Dictionary<ulong, string> field13, global::System.Collections.Generic.Dictionary<int, string> field14, global::System.Collections.Generic.Dictionary<long, string> field15, global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string> field16, global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string> field17, global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string> field18)
            {
                Field1 = field1;
                Field2 = field2;
                Field3 = field3;
                Field4 = field4;
                Field5 = field5;
                Field6 = field6;
                Field7 = field7;
                Field8 = field8;
                Field9 = field9;
                Field10 = field10;
                Field11 = field11;
                Field12 = field12;
                Field13 = field13;
                Field14 = field14;
                Field15 = field15;
                Field16 = field16;
                Field17 = field17;
                Field18 = field18;
            }
        }

        public static class Serialization
        {
            public static void SerializeComponent(global::Improbable.TestSchema.ExhaustiveMapKey.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                foreach (var keyValuePair in component.Field1)
                {
                    var mapObj = obj.AddObject(1);
                    mapObj.AddBool(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field2)
                {
                    var mapObj = obj.AddObject(2);
                    mapObj.AddFloat(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field3)
                {
                    var mapObj = obj.AddObject(3);
                    mapObj.AddBytes(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field4)
                {
                    var mapObj = obj.AddObject(4);
                    mapObj.AddInt32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field5)
                {
                    var mapObj = obj.AddObject(5);
                    mapObj.AddInt64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field6)
                {
                    var mapObj = obj.AddObject(6);
                    mapObj.AddDouble(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field7)
                {
                    var mapObj = obj.AddObject(7);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field8)
                {
                    var mapObj = obj.AddObject(8);
                    mapObj.AddUint32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field9)
                {
                    var mapObj = obj.AddObject(9);
                    mapObj.AddUint64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field10)
                {
                    var mapObj = obj.AddObject(10);
                    mapObj.AddSint32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field11)
                {
                    var mapObj = obj.AddObject(11);
                    mapObj.AddSint64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field12)
                {
                    var mapObj = obj.AddObject(12);
                    mapObj.AddFixed32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field13)
                {
                    var mapObj = obj.AddObject(13);
                    mapObj.AddFixed64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field14)
                {
                    var mapObj = obj.AddObject(14);
                    mapObj.AddSfixed32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field15)
                {
                    var mapObj = obj.AddObject(15);
                    mapObj.AddSfixed64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field16)
                {
                    var mapObj = obj.AddObject(16);
                    mapObj.AddEntityId(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field17)
                {
                    var mapObj = obj.AddObject(17);
                    global::Improbable.TestSchema.SomeType.Serialization.Serialize(keyValuePair.Key, mapObj.AddObject(1));
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in component.Field18)
                {
                    var mapObj = obj.AddObject(18);
                    mapObj.AddEnum(1, (uint) keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
            }

            public static void SerializeUpdate(global::Improbable.TestSchema.ExhaustiveMapKey.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                if (component.IsDataDirty(0))
                {
                    foreach (var keyValuePair in component.Field1)
                    {
                        var mapObj = obj.AddObject(1);
                        mapObj.AddBool(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field1.Count == 0)
                    {
                        updateObj.AddClearedField(1);
                    }
                }

                if (component.IsDataDirty(1))
                {
                    foreach (var keyValuePair in component.Field2)
                    {
                        var mapObj = obj.AddObject(2);
                        mapObj.AddFloat(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field2.Count == 0)
                    {
                        updateObj.AddClearedField(2);
                    }
                }

                if (component.IsDataDirty(2))
                {
                    foreach (var keyValuePair in component.Field3)
                    {
                        var mapObj = obj.AddObject(3);
                        mapObj.AddBytes(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field3.Count == 0)
                    {
                        updateObj.AddClearedField(3);
                    }
                }

                if (component.IsDataDirty(3))
                {
                    foreach (var keyValuePair in component.Field4)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddInt32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field4.Count == 0)
                    {
                        updateObj.AddClearedField(4);
                    }
                }

                if (component.IsDataDirty(4))
                {
                    foreach (var keyValuePair in component.Field5)
                    {
                        var mapObj = obj.AddObject(5);
                        mapObj.AddInt64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field5.Count == 0)
                    {
                        updateObj.AddClearedField(5);
                    }
                }

                if (component.IsDataDirty(5))
                {
                    foreach (var keyValuePair in component.Field6)
                    {
                        var mapObj = obj.AddObject(6);
                        mapObj.AddDouble(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field6.Count == 0)
                    {
                        updateObj.AddClearedField(6);
                    }
                }

                if (component.IsDataDirty(6))
                {
                    foreach (var keyValuePair in component.Field7)
                    {
                        var mapObj = obj.AddObject(7);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field7.Count == 0)
                    {
                        updateObj.AddClearedField(7);
                    }
                }

                if (component.IsDataDirty(7))
                {
                    foreach (var keyValuePair in component.Field8)
                    {
                        var mapObj = obj.AddObject(8);
                        mapObj.AddUint32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field8.Count == 0)
                    {
                        updateObj.AddClearedField(8);
                    }
                }

                if (component.IsDataDirty(8))
                {
                    foreach (var keyValuePair in component.Field9)
                    {
                        var mapObj = obj.AddObject(9);
                        mapObj.AddUint64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field9.Count == 0)
                    {
                        updateObj.AddClearedField(9);
                    }
                }

                if (component.IsDataDirty(9))
                {
                    foreach (var keyValuePair in component.Field10)
                    {
                        var mapObj = obj.AddObject(10);
                        mapObj.AddSint32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field10.Count == 0)
                    {
                        updateObj.AddClearedField(10);
                    }
                }

                if (component.IsDataDirty(10))
                {
                    foreach (var keyValuePair in component.Field11)
                    {
                        var mapObj = obj.AddObject(11);
                        mapObj.AddSint64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field11.Count == 0)
                    {
                        updateObj.AddClearedField(11);
                    }
                }

                if (component.IsDataDirty(11))
                {
                    foreach (var keyValuePair in component.Field12)
                    {
                        var mapObj = obj.AddObject(12);
                        mapObj.AddFixed32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field12.Count == 0)
                    {
                        updateObj.AddClearedField(12);
                    }
                }

                if (component.IsDataDirty(12))
                {
                    foreach (var keyValuePair in component.Field13)
                    {
                        var mapObj = obj.AddObject(13);
                        mapObj.AddFixed64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field13.Count == 0)
                    {
                        updateObj.AddClearedField(13);
                    }
                }

                if (component.IsDataDirty(13))
                {
                    foreach (var keyValuePair in component.Field14)
                    {
                        var mapObj = obj.AddObject(14);
                        mapObj.AddSfixed32(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field14.Count == 0)
                    {
                        updateObj.AddClearedField(14);
                    }
                }

                if (component.IsDataDirty(14))
                {
                    foreach (var keyValuePair in component.Field15)
                    {
                        var mapObj = obj.AddObject(15);
                        mapObj.AddSfixed64(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field15.Count == 0)
                    {
                        updateObj.AddClearedField(15);
                    }
                }

                if (component.IsDataDirty(15))
                {
                    foreach (var keyValuePair in component.Field16)
                    {
                        var mapObj = obj.AddObject(16);
                        mapObj.AddEntityId(1, keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field16.Count == 0)
                    {
                        updateObj.AddClearedField(16);
                    }
                }

                if (component.IsDataDirty(16))
                {
                    foreach (var keyValuePair in component.Field17)
                    {
                        var mapObj = obj.AddObject(17);
                        global::Improbable.TestSchema.SomeType.Serialization.Serialize(keyValuePair.Key, mapObj.AddObject(1));
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field17.Count == 0)
                    {
                        updateObj.AddClearedField(17);
                    }
                }

                if (component.IsDataDirty(17))
                {
                    foreach (var keyValuePair in component.Field18)
                    {
                        var mapObj = obj.AddObject(18);
                        mapObj.AddEnum(1, (uint) keyValuePair.Key);
                        mapObj.AddString(2, keyValuePair.Value);
                    }

                    if (component.Field18.Count == 0)
                    {
                        updateObj.AddClearedField(18);
                    }
                }
            }

            public static void SerializeUpdate(global::Improbable.TestSchema.ExhaustiveMapKey.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();

                {
                    if (update.Field1.HasValue)
                    {
                        var field = update.Field1.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(1);
                            mapObj.AddBool(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(1);
                        }
                    }
                }

                {
                    if (update.Field2.HasValue)
                    {
                        var field = update.Field2.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(2);
                            mapObj.AddFloat(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(2);
                        }
                    }
                }

                {
                    if (update.Field3.HasValue)
                    {
                        var field = update.Field3.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(3);
                            mapObj.AddBytes(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(3);
                        }
                    }
                }

                {
                    if (update.Field4.HasValue)
                    {
                        var field = update.Field4.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(4);
                            mapObj.AddInt32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(4);
                        }
                    }
                }

                {
                    if (update.Field5.HasValue)
                    {
                        var field = update.Field5.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(5);
                            mapObj.AddInt64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(5);
                        }
                    }
                }

                {
                    if (update.Field6.HasValue)
                    {
                        var field = update.Field6.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(6);
                            mapObj.AddDouble(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(6);
                        }
                    }
                }

                {
                    if (update.Field7.HasValue)
                    {
                        var field = update.Field7.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(7);
                            mapObj.AddString(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(7);
                        }
                    }
                }

                {
                    if (update.Field8.HasValue)
                    {
                        var field = update.Field8.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(8);
                            mapObj.AddUint32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(8);
                        }
                    }
                }

                {
                    if (update.Field9.HasValue)
                    {
                        var field = update.Field9.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(9);
                            mapObj.AddUint64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(9);
                        }
                    }
                }

                {
                    if (update.Field10.HasValue)
                    {
                        var field = update.Field10.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(10);
                            mapObj.AddSint32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(10);
                        }
                    }
                }

                {
                    if (update.Field11.HasValue)
                    {
                        var field = update.Field11.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(11);
                            mapObj.AddSint64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(11);
                        }
                    }
                }

                {
                    if (update.Field12.HasValue)
                    {
                        var field = update.Field12.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(12);
                            mapObj.AddFixed32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(12);
                        }
                    }
                }

                {
                    if (update.Field13.HasValue)
                    {
                        var field = update.Field13.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(13);
                            mapObj.AddFixed64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(13);
                        }
                    }
                }

                {
                    if (update.Field14.HasValue)
                    {
                        var field = update.Field14.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(14);
                            mapObj.AddSfixed32(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(14);
                        }
                    }
                }

                {
                    if (update.Field15.HasValue)
                    {
                        var field = update.Field15.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(15);
                            mapObj.AddSfixed64(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(15);
                        }
                    }
                }

                {
                    if (update.Field16.HasValue)
                    {
                        var field = update.Field16.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(16);
                            mapObj.AddEntityId(1, keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(16);
                        }
                    }
                }

                {
                    if (update.Field17.HasValue)
                    {
                        var field = update.Field17.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(17);
                            global::Improbable.TestSchema.SomeType.Serialization.Serialize(keyValuePair.Key, mapObj.AddObject(1));
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(17);
                        }
                    }
                }

                {
                    if (update.Field18.HasValue)
                    {
                        var field = update.Field18.Value;

                        foreach (var keyValuePair in field)
                        {
                            var mapObj = obj.AddObject(18);
                            mapObj.AddEnum(1, (uint) keyValuePair.Key);
                            mapObj.AddString(2, keyValuePair.Value);
                        }

                        if (field.Count == 0)
                        {
                            updateObj.AddClearedField(18);
                        }
                    }
                }
            }

            public static void SerializeSnapshot(global::Improbable.TestSchema.ExhaustiveMapKey.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                foreach (var keyValuePair in snapshot.Field1)
                {
                    var mapObj = obj.AddObject(1);
                    mapObj.AddBool(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field2)
                {
                    var mapObj = obj.AddObject(2);
                    mapObj.AddFloat(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field3)
                {
                    var mapObj = obj.AddObject(3);
                    mapObj.AddBytes(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field4)
                {
                    var mapObj = obj.AddObject(4);
                    mapObj.AddInt32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field5)
                {
                    var mapObj = obj.AddObject(5);
                    mapObj.AddInt64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field6)
                {
                    var mapObj = obj.AddObject(6);
                    mapObj.AddDouble(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field7)
                {
                    var mapObj = obj.AddObject(7);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field8)
                {
                    var mapObj = obj.AddObject(8);
                    mapObj.AddUint32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field9)
                {
                    var mapObj = obj.AddObject(9);
                    mapObj.AddUint64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field10)
                {
                    var mapObj = obj.AddObject(10);
                    mapObj.AddSint32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field11)
                {
                    var mapObj = obj.AddObject(11);
                    mapObj.AddSint64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field12)
                {
                    var mapObj = obj.AddObject(12);
                    mapObj.AddFixed32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field13)
                {
                    var mapObj = obj.AddObject(13);
                    mapObj.AddFixed64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field14)
                {
                    var mapObj = obj.AddObject(14);
                    mapObj.AddSfixed32(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field15)
                {
                    var mapObj = obj.AddObject(15);
                    mapObj.AddSfixed64(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field16)
                {
                    var mapObj = obj.AddObject(16);
                    mapObj.AddEntityId(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field17)
                {
                    var mapObj = obj.AddObject(17);
                    global::Improbable.TestSchema.SomeType.Serialization.Serialize(keyValuePair.Key, mapObj.AddObject(1));
                    mapObj.AddString(2, keyValuePair.Value);
                }

                foreach (var keyValuePair in snapshot.Field18)
                {
                    var mapObj = obj.AddObject(18);
                    mapObj.AddEnum(1, (uint) keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
            }

            public static global::Improbable.TestSchema.ExhaustiveMapKey.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new global::Improbable.TestSchema.ExhaustiveMapKey.Component();

                component.field1Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<bool, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<bool, string>();
                    var mapSize = obj.GetObjectCount(1);
                    component.Field1 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field2Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<float, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<float, string>();
                    var mapSize = obj.GetObjectCount(2);
                    component.Field2 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field3Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<byte[], string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<byte[], string>();
                    var mapSize = obj.GetObjectCount(3);
                    component.Field3 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field4Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<int, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<int, string>();
                    var mapSize = obj.GetObjectCount(4);
                    component.Field4 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field5Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<long, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<long, string>();
                    var mapSize = obj.GetObjectCount(5);
                    component.Field5 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field6Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<double, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<double, string>();
                    var mapSize = obj.GetObjectCount(6);
                    component.Field6 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field7Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<string, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<string, string>();
                    var mapSize = obj.GetObjectCount(7);
                    component.Field7 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field8Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<uint, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<uint, string>();
                    var mapSize = obj.GetObjectCount(8);
                    component.Field8 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field9Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<ulong, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<ulong, string>();
                    var mapSize = obj.GetObjectCount(9);
                    component.Field9 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field10Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<int, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<int, string>();
                    var mapSize = obj.GetObjectCount(10);
                    component.Field10 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field11Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<long, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<long, string>();
                    var mapSize = obj.GetObjectCount(11);
                    component.Field11 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field12Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<uint, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<uint, string>();
                    var mapSize = obj.GetObjectCount(12);
                    component.Field12 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field13Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<ulong, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<ulong, string>();
                    var mapSize = obj.GetObjectCount(13);
                    component.Field13 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field14Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<int, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<int, string>();
                    var mapSize = obj.GetObjectCount(14);
                    component.Field14 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field15Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<long, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<long, string>();
                    var mapSize = obj.GetObjectCount(15);
                    component.Field15 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field16Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>();
                    var mapSize = obj.GetObjectCount(16);
                    component.Field16 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field17Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>();
                    var mapSize = obj.GetObjectCount(17);
                    component.Field17 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.TestSchema.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                component.field18Handle = global::Improbable.Gdk.Core.ReferenceProvider<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>>.Create();

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>();
                    var mapSize = obj.GetObjectCount(18);
                    component.Field18 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.TestSchema.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                return component;
            }

            public static global::Improbable.TestSchema.ExhaustiveMapKey.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)
            {
                var update = new global::Improbable.TestSchema.ExhaustiveMapKey.Update();
                var obj = updateObj.GetFields();

                {
                    var mapSize = obj.GetObjectCount(1);

                    var isCleared = updateObj.IsFieldCleared(1);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field1 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<bool, string>>(new global::System.Collections.Generic.Dictionary<bool, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        update.Field1.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(2);

                    var isCleared = updateObj.IsFieldCleared(2);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field2 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<float, string>>(new global::System.Collections.Generic.Dictionary<float, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        update.Field2.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(3);

                    var isCleared = updateObj.IsFieldCleared(3);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field3 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<byte[], string>>(new global::System.Collections.Generic.Dictionary<byte[], string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        update.Field3.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(4);

                    var isCleared = updateObj.IsFieldCleared(4);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field4 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<int, string>>(new global::System.Collections.Generic.Dictionary<int, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        update.Field4.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(5);

                    var isCleared = updateObj.IsFieldCleared(5);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field5 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<long, string>>(new global::System.Collections.Generic.Dictionary<long, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        update.Field5.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(6);

                    var isCleared = updateObj.IsFieldCleared(6);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field6 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<double, string>>(new global::System.Collections.Generic.Dictionary<double, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        update.Field6.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(7);

                    var isCleared = updateObj.IsFieldCleared(7);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field7 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string, string>>(new global::System.Collections.Generic.Dictionary<string, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        update.Field7.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(8);

                    var isCleared = updateObj.IsFieldCleared(8);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field8 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<uint, string>>(new global::System.Collections.Generic.Dictionary<uint, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        update.Field8.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(9);

                    var isCleared = updateObj.IsFieldCleared(9);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field9 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<ulong, string>>(new global::System.Collections.Generic.Dictionary<ulong, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        update.Field9.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(10);

                    var isCleared = updateObj.IsFieldCleared(10);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field10 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<int, string>>(new global::System.Collections.Generic.Dictionary<int, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        update.Field10.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(11);

                    var isCleared = updateObj.IsFieldCleared(11);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field11 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<long, string>>(new global::System.Collections.Generic.Dictionary<long, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        update.Field11.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(12);

                    var isCleared = updateObj.IsFieldCleared(12);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field12 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<uint, string>>(new global::System.Collections.Generic.Dictionary<uint, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        update.Field12.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(13);

                    var isCleared = updateObj.IsFieldCleared(13);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field13 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<ulong, string>>(new global::System.Collections.Generic.Dictionary<ulong, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        update.Field13.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(14);

                    var isCleared = updateObj.IsFieldCleared(14);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field14 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<int, string>>(new global::System.Collections.Generic.Dictionary<int, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        update.Field14.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(15);

                    var isCleared = updateObj.IsFieldCleared(15);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field15 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<long, string>>(new global::System.Collections.Generic.Dictionary<long, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        update.Field15.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(16);

                    var isCleared = updateObj.IsFieldCleared(16);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field16 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>>(new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        update.Field16.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(17);

                    var isCleared = updateObj.IsFieldCleared(17);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field17 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>>(new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.TestSchema.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        update.Field17.Value.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(18);

                    var isCleared = updateObj.IsFieldCleared(18);

                    if (mapSize > 0 || isCleared)
                    {
                        update.Field18 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>>(new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>());
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.TestSchema.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        update.Field18.Value.Add(key, value);
                    }
                }

                return update;
            }

            public static global::Improbable.TestSchema.ExhaustiveMapKey.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)
            {
                var update = new global::Improbable.TestSchema.ExhaustiveMapKey.Update();
                var obj = data.GetFields();

                {
                    var map = new global::System.Collections.Generic.Dictionary<bool, string>();
                    var mapSize = obj.GetObjectCount(1);
                    update.Field1 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<float, string>();
                    var mapSize = obj.GetObjectCount(2);
                    update.Field2 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<byte[], string>();
                    var mapSize = obj.GetObjectCount(3);
                    update.Field3 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<int, string>();
                    var mapSize = obj.GetObjectCount(4);
                    update.Field4 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<long, string>();
                    var mapSize = obj.GetObjectCount(5);
                    update.Field5 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<double, string>();
                    var mapSize = obj.GetObjectCount(6);
                    update.Field6 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<string, string>();
                    var mapSize = obj.GetObjectCount(7);
                    update.Field7 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<uint, string>();
                    var mapSize = obj.GetObjectCount(8);
                    update.Field8 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<ulong, string>();
                    var mapSize = obj.GetObjectCount(9);
                    update.Field9 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<int, string>();
                    var mapSize = obj.GetObjectCount(10);
                    update.Field10 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<long, string>();
                    var mapSize = obj.GetObjectCount(11);
                    update.Field11 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<uint, string>();
                    var mapSize = obj.GetObjectCount(12);
                    update.Field12 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<ulong, string>();
                    var mapSize = obj.GetObjectCount(13);
                    update.Field13 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<int, string>();
                    var mapSize = obj.GetObjectCount(14);
                    update.Field14 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<long, string>();
                    var mapSize = obj.GetObjectCount(15);
                    update.Field15 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>();
                    var mapSize = obj.GetObjectCount(16);
                    update.Field16 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>();
                    var mapSize = obj.GetObjectCount(17);
                    update.Field17 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.TestSchema.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>();
                    var mapSize = obj.GetObjectCount(18);
                    update.Field18 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.TestSchema.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                return update;
            }

            public static global::Improbable.TestSchema.ExhaustiveMapKey.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)
            {
                var component = new global::Improbable.TestSchema.ExhaustiveMapKey.Snapshot();

                {
                    var map = new global::System.Collections.Generic.Dictionary<bool, string>();
                    var mapSize = obj.GetObjectCount(1);
                    component.Field1 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<float, string>();
                    var mapSize = obj.GetObjectCount(2);
                    component.Field2 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<byte[], string>();
                    var mapSize = obj.GetObjectCount(3);
                    component.Field3 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<int, string>();
                    var mapSize = obj.GetObjectCount(4);
                    component.Field4 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<long, string>();
                    var mapSize = obj.GetObjectCount(5);
                    component.Field5 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<double, string>();
                    var mapSize = obj.GetObjectCount(6);
                    component.Field6 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<string, string>();
                    var mapSize = obj.GetObjectCount(7);
                    component.Field7 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<uint, string>();
                    var mapSize = obj.GetObjectCount(8);
                    component.Field8 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<ulong, string>();
                    var mapSize = obj.GetObjectCount(9);
                    component.Field9 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<int, string>();
                    var mapSize = obj.GetObjectCount(10);
                    component.Field10 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<long, string>();
                    var mapSize = obj.GetObjectCount(11);
                    component.Field11 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<uint, string>();
                    var mapSize = obj.GetObjectCount(12);
                    component.Field12 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<ulong, string>();
                    var mapSize = obj.GetObjectCount(13);
                    component.Field13 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<int, string>();
                    var mapSize = obj.GetObjectCount(14);
                    component.Field14 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<long, string>();
                    var mapSize = obj.GetObjectCount(15);
                    component.Field15 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>();
                    var mapSize = obj.GetObjectCount(16);
                    component.Field16 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>();
                    var mapSize = obj.GetObjectCount(17);
                    component.Field17 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.TestSchema.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                {
                    var map = new global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>();
                    var mapSize = obj.GetObjectCount(18);
                    component.Field18 = map;

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.TestSchema.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                }

                return component;
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.TestSchema.ExhaustiveMapKey.Component component)
            {
                var obj = updateObj.GetFields();

                {
                    var mapSize = obj.GetObjectCount(1);

                    var isCleared = updateObj.IsFieldCleared(1);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field1.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        component.Field1.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(2);

                    var isCleared = updateObj.IsFieldCleared(2);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field2.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        component.Field2.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(3);

                    var isCleared = updateObj.IsFieldCleared(3);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field3.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        component.Field3.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(4);

                    var isCleared = updateObj.IsFieldCleared(4);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field4.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        component.Field4.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(5);

                    var isCleared = updateObj.IsFieldCleared(5);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field5.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        component.Field5.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(6);

                    var isCleared = updateObj.IsFieldCleared(6);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field6.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        component.Field6.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(7);

                    var isCleared = updateObj.IsFieldCleared(7);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field7.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        component.Field7.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(8);

                    var isCleared = updateObj.IsFieldCleared(8);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field8.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        component.Field8.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(9);

                    var isCleared = updateObj.IsFieldCleared(9);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field9.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        component.Field9.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(10);

                    var isCleared = updateObj.IsFieldCleared(10);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field10.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        component.Field10.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(11);

                    var isCleared = updateObj.IsFieldCleared(11);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field11.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        component.Field11.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(12);

                    var isCleared = updateObj.IsFieldCleared(12);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field12.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        component.Field12.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(13);

                    var isCleared = updateObj.IsFieldCleared(13);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field13.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        component.Field13.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(14);

                    var isCleared = updateObj.IsFieldCleared(14);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field14.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        component.Field14.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(15);

                    var isCleared = updateObj.IsFieldCleared(15);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field15.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        component.Field15.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(16);

                    var isCleared = updateObj.IsFieldCleared(16);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field16.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        component.Field16.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(17);

                    var isCleared = updateObj.IsFieldCleared(17);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field17.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.TestSchema.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        component.Field17.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(18);

                    var isCleared = updateObj.IsFieldCleared(18);

                    if (mapSize > 0 || isCleared)
                    {
                        component.Field18.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.TestSchema.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        component.Field18.Add(key, value);
                    }
                }
            }

            public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref global::Improbable.TestSchema.ExhaustiveMapKey.Snapshot snapshot)
            {
                var obj = updateObj.GetFields();

                {
                    var mapSize = obj.GetObjectCount(1);

                    var isCleared = updateObj.IsFieldCleared(1);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field1.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetBool(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field1.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(2);

                    var isCleared = updateObj.IsFieldCleared(2);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field2.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetFloat(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field2.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(3);

                    var isCleared = updateObj.IsFieldCleared(3);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field3.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetBytes(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field3.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(4);

                    var isCleared = updateObj.IsFieldCleared(4);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field4.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetInt32(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field4.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(5);

                    var isCleared = updateObj.IsFieldCleared(5);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field5.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetInt64(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field5.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(6);

                    var isCleared = updateObj.IsFieldCleared(6);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field6.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetDouble(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field6.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(7);

                    var isCleared = updateObj.IsFieldCleared(7);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field7.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field7.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(8);

                    var isCleared = updateObj.IsFieldCleared(8);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field8.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetUint32(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field8.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(9);

                    var isCleared = updateObj.IsFieldCleared(9);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field9.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetUint64(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field9.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(10);

                    var isCleared = updateObj.IsFieldCleared(10);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field10.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetSint32(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field10.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(11);

                    var isCleared = updateObj.IsFieldCleared(11);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field11.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetSint64(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field11.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(12);

                    var isCleared = updateObj.IsFieldCleared(12);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field12.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetFixed32(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field12.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(13);

                    var isCleared = updateObj.IsFieldCleared(13);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field13.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetFixed64(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field13.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(14);

                    var isCleared = updateObj.IsFieldCleared(14);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field14.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetSfixed32(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field14.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(15);

                    var isCleared = updateObj.IsFieldCleared(15);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field15.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetSfixed64(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field15.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(16);

                    var isCleared = updateObj.IsFieldCleared(16);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field16.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetEntityIdStruct(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field16.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(17);

                    var isCleared = updateObj.IsFieldCleared(17);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field17.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = global::Improbable.TestSchema.SomeType.Serialization.Deserialize(mapObj.GetObject(1));
                        var value = mapObj.GetString(2);
                        snapshot.Field17.Add(key, value);
                    }
                }

                {
                    var mapSize = obj.GetObjectCount(18);

                    var isCleared = updateObj.IsFieldCleared(18);

                    if (mapSize > 0 || isCleared)
                    {
                        snapshot.Field18.Clear();
                    }

                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(18, (uint) i);
                        var key = (global::Improbable.TestSchema.SomeEnum) mapObj.GetEnum(1);
                        var value = mapObj.GetString(2);
                        snapshot.Field18.Add(key, value);
                    }
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            public Option<global::System.Collections.Generic.Dictionary<bool, string>> Field1;
            public Option<global::System.Collections.Generic.Dictionary<float, string>> Field2;
            public Option<global::System.Collections.Generic.Dictionary<byte[], string>> Field3;
            public Option<global::System.Collections.Generic.Dictionary<int, string>> Field4;
            public Option<global::System.Collections.Generic.Dictionary<long, string>> Field5;
            public Option<global::System.Collections.Generic.Dictionary<double, string>> Field6;
            public Option<global::System.Collections.Generic.Dictionary<string, string>> Field7;
            public Option<global::System.Collections.Generic.Dictionary<uint, string>> Field8;
            public Option<global::System.Collections.Generic.Dictionary<ulong, string>> Field9;
            public Option<global::System.Collections.Generic.Dictionary<int, string>> Field10;
            public Option<global::System.Collections.Generic.Dictionary<long, string>> Field11;
            public Option<global::System.Collections.Generic.Dictionary<uint, string>> Field12;
            public Option<global::System.Collections.Generic.Dictionary<ulong, string>> Field13;
            public Option<global::System.Collections.Generic.Dictionary<int, string>> Field14;
            public Option<global::System.Collections.Generic.Dictionary<long, string>> Field15;
            public Option<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntityId, string>> Field16;
            public Option<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeType, string>> Field17;
            public Option<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, string>> Field18;
        }

        internal class ExhaustiveMapKeyDynamic : IDynamicInvokable
        {
            public uint ComponentId => ExhaustiveMapKey.ComponentId;

            internal static Dynamic.VTable<Update, Snapshot> VTable = new Dynamic.VTable<Update, Snapshot>
            {
                DeserializeSnapshot = DeserializeSnapshot,
                SerializeSnapshot = SerializeSnapshot,
                DeserializeSnapshotRaw = Serialization.DeserializeSnapshot,
                SerializeSnapshotRaw = Serialization.SerializeSnapshot,
                ConvertSnapshotToUpdate = SnapshotToUpdate
            };

            private static Snapshot DeserializeSnapshot(ComponentData snapshot)
            {
                var schemaDataOpt = snapshot.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.DeserializeSnapshot(schemaDataOpt.Value.GetFields());
            }

            private static void SerializeSnapshot(Snapshot snapshot, ComponentData data)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not serialise an empty {nameof(ComponentData)}");
                }

                Serialization.SerializeSnapshot(snapshot, data.SchemaData.Value.GetFields());
            }

            private static Update SnapshotToUpdate(in Snapshot snapshot)
            {
                var update = new Update
                {
                    Field1 = snapshot.Field1,
                    Field2 = snapshot.Field2,
                    Field3 = snapshot.Field3,
                    Field4 = snapshot.Field4,
                    Field5 = snapshot.Field5,
                    Field6 = snapshot.Field6,
                    Field7 = snapshot.Field7,
                    Field8 = snapshot.Field8,
                    Field9 = snapshot.Field9,
                    Field10 = snapshot.Field10,
                    Field11 = snapshot.Field11,
                    Field12 = snapshot.Field12,
                    Field13 = snapshot.Field13,
                    Field14 = snapshot.Field14,
                    Field15 = snapshot.Field15,
                    Field16 = snapshot.Field16,
                    Field17 = snapshot.Field17,
                    Field18 = snapshot.Field18
                };

                return update;
            }

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Update, Snapshot>(ComponentId, VTable);
            }
        }
    }
}
