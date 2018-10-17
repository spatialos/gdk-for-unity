// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveMapValue
    {
        public const uint ComponentId = 197718;

        public struct Component : IComponentData, ISpatialComponentData
        {
            public uint ComponentId => 197718;

            public BlittableBool DirtyBit { get; set; }

            internal uint field1Handle;

            public global::System.Collections.Generic.Dictionary<string,BlittableBool> Field1
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field1Provider.Get(field1Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field1Provider.Set(field1Handle, value);
                }
            }

            internal uint field2Handle;

            public global::System.Collections.Generic.Dictionary<string,float> Field2
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field2Provider.Get(field2Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field2Provider.Set(field2Handle, value);
                }
            }

            internal uint field3Handle;

            public global::System.Collections.Generic.Dictionary<string,byte[]> Field3
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field3Provider.Get(field3Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field3Provider.Set(field3Handle, value);
                }
            }

            internal uint field4Handle;

            public global::System.Collections.Generic.Dictionary<string,int> Field4
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field4Provider.Get(field4Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field4Provider.Set(field4Handle, value);
                }
            }

            internal uint field5Handle;

            public global::System.Collections.Generic.Dictionary<string,long> Field5
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field5Provider.Get(field5Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field5Provider.Set(field5Handle, value);
                }
            }

            internal uint field6Handle;

            public global::System.Collections.Generic.Dictionary<string,double> Field6
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field6Provider.Get(field6Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field6Provider.Set(field6Handle, value);
                }
            }

            internal uint field7Handle;

            public global::System.Collections.Generic.Dictionary<string,string> Field7
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field7Provider.Get(field7Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field7Provider.Set(field7Handle, value);
                }
            }

            internal uint field8Handle;

            public global::System.Collections.Generic.Dictionary<string,uint> Field8
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field8Provider.Get(field8Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field8Provider.Set(field8Handle, value);
                }
            }

            internal uint field9Handle;

            public global::System.Collections.Generic.Dictionary<string,ulong> Field9
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field9Provider.Get(field9Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field9Provider.Set(field9Handle, value);
                }
            }

            internal uint field10Handle;

            public global::System.Collections.Generic.Dictionary<string,int> Field10
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field10Provider.Get(field10Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field10Provider.Set(field10Handle, value);
                }
            }

            internal uint field11Handle;

            public global::System.Collections.Generic.Dictionary<string,long> Field11
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field11Provider.Get(field11Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field11Provider.Set(field11Handle, value);
                }
            }

            internal uint field12Handle;

            public global::System.Collections.Generic.Dictionary<string,uint> Field12
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field12Provider.Get(field12Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field12Provider.Set(field12Handle, value);
                }
            }

            internal uint field13Handle;

            public global::System.Collections.Generic.Dictionary<string,ulong> Field13
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field13Provider.Get(field13Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field13Provider.Set(field13Handle, value);
                }
            }

            internal uint field14Handle;

            public global::System.Collections.Generic.Dictionary<string,int> Field14
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field14Provider.Get(field14Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field14Provider.Set(field14Handle, value);
                }
            }

            internal uint field15Handle;

            public global::System.Collections.Generic.Dictionary<string,long> Field15
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field15Provider.Get(field15Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field15Provider.Set(field15Handle, value);
                }
            }

            internal uint field16Handle;

            public global::System.Collections.Generic.Dictionary<string,global::Improbable.Worker.EntityId> Field16
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field16Provider.Get(field16Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field16Provider.Set(field16Handle, value);
                }
            }

            internal uint field17Handle;

            public global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType> Field17
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field17Provider.Get(field17Handle);
                set
                {
                    DirtyBit = true;
                    Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field17Provider.Set(field17Handle, value);
                }
            }

            public static global::Improbable.Worker.Core.ComponentData CreateSchemaComponentData(
                global::System.Collections.Generic.Dictionary<string,BlittableBool> field1,
                global::System.Collections.Generic.Dictionary<string,float> field2,
                global::System.Collections.Generic.Dictionary<string,byte[]> field3,
                global::System.Collections.Generic.Dictionary<string,int> field4,
                global::System.Collections.Generic.Dictionary<string,long> field5,
                global::System.Collections.Generic.Dictionary<string,double> field6,
                global::System.Collections.Generic.Dictionary<string,string> field7,
                global::System.Collections.Generic.Dictionary<string,uint> field8,
                global::System.Collections.Generic.Dictionary<string,ulong> field9,
                global::System.Collections.Generic.Dictionary<string,int> field10,
                global::System.Collections.Generic.Dictionary<string,long> field11,
                global::System.Collections.Generic.Dictionary<string,uint> field12,
                global::System.Collections.Generic.Dictionary<string,ulong> field13,
                global::System.Collections.Generic.Dictionary<string,int> field14,
                global::System.Collections.Generic.Dictionary<string,long> field15,
                global::System.Collections.Generic.Dictionary<string,global::Improbable.Worker.EntityId> field16,
                global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType> field17
        )
            {
                var schemaComponentData = new global::Improbable.Worker.Core.SchemaComponentData(197718);
                var obj = schemaComponentData.GetFields();
                {
                    foreach (var keyValuePair in field1)
                {
                    var mapObj = obj.AddObject(1);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddBool(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field2)
                {
                    var mapObj = obj.AddObject(2);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddFloat(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field3)
                {
                    var mapObj = obj.AddObject(3);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddBytes(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field4)
                {
                    var mapObj = obj.AddObject(4);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddInt32(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field5)
                {
                    var mapObj = obj.AddObject(5);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddInt64(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field6)
                {
                    var mapObj = obj.AddObject(6);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddDouble(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field7)
                {
                    var mapObj = obj.AddObject(7);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddString(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field8)
                {
                    var mapObj = obj.AddObject(8);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddUint32(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field9)
                {
                    var mapObj = obj.AddObject(9);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddUint64(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field10)
                {
                    var mapObj = obj.AddObject(10);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddSint32(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field11)
                {
                    var mapObj = obj.AddObject(11);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddSint64(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field12)
                {
                    var mapObj = obj.AddObject(12);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddFixed32(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field13)
                {
                    var mapObj = obj.AddObject(13);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddFixed64(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field14)
                {
                    var mapObj = obj.AddObject(14);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddSfixed32(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field15)
                {
                    var mapObj = obj.AddObject(15);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddSfixed64(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field16)
                {
                    var mapObj = obj.AddObject(16);
                    mapObj.AddString(1, keyValuePair.Key);
                    mapObj.AddEntityId(2, keyValuePair.Value);
                }
                
                }
                {
                    foreach (var keyValuePair in field17)
                {
                    var mapObj = obj.AddObject(17);
                    mapObj.AddString(1, keyValuePair.Key);
                    global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                }
                
                }
                return new global::Improbable.Worker.Core.ComponentData(schemaComponentData);
            }
        }

        public static class Serialization
        {
            public static void SerializeUpdate(Improbable.Gdk.Tests.ExhaustiveMapValue.Component component, global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var obj = updateObj.GetFields();
                {
                    foreach (var keyValuePair in component.Field1)
                    {
                        var mapObj = obj.AddObject(1);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddBool(2, keyValuePair.Value);
                    }
                    
                    if (component.Field1.Count == 0)
                    {
                        updateObj.AddClearedField(1);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field2)
                    {
                        var mapObj = obj.AddObject(2);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddFloat(2, keyValuePair.Value);
                    }
                    
                    if (component.Field2.Count == 0)
                    {
                        updateObj.AddClearedField(2);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field3)
                    {
                        var mapObj = obj.AddObject(3);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddBytes(2, keyValuePair.Value);
                    }
                    
                    if (component.Field3.Count == 0)
                    {
                        updateObj.AddClearedField(3);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field4)
                    {
                        var mapObj = obj.AddObject(4);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddInt32(2, keyValuePair.Value);
                    }
                    
                    if (component.Field4.Count == 0)
                    {
                        updateObj.AddClearedField(4);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field5)
                    {
                        var mapObj = obj.AddObject(5);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddInt64(2, keyValuePair.Value);
                    }
                    
                    if (component.Field5.Count == 0)
                    {
                        updateObj.AddClearedField(5);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field6)
                    {
                        var mapObj = obj.AddObject(6);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddDouble(2, keyValuePair.Value);
                    }
                    
                    if (component.Field6.Count == 0)
                    {
                        updateObj.AddClearedField(6);
                    }
                    
                }
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
                {
                    foreach (var keyValuePair in component.Field8)
                    {
                        var mapObj = obj.AddObject(8);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddUint32(2, keyValuePair.Value);
                    }
                    
                    if (component.Field8.Count == 0)
                    {
                        updateObj.AddClearedField(8);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field9)
                    {
                        var mapObj = obj.AddObject(9);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddUint64(2, keyValuePair.Value);
                    }
                    
                    if (component.Field9.Count == 0)
                    {
                        updateObj.AddClearedField(9);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field10)
                    {
                        var mapObj = obj.AddObject(10);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddSint32(2, keyValuePair.Value);
                    }
                    
                    if (component.Field10.Count == 0)
                    {
                        updateObj.AddClearedField(10);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field11)
                    {
                        var mapObj = obj.AddObject(11);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddSint64(2, keyValuePair.Value);
                    }
                    
                    if (component.Field11.Count == 0)
                    {
                        updateObj.AddClearedField(11);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field12)
                    {
                        var mapObj = obj.AddObject(12);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddFixed32(2, keyValuePair.Value);
                    }
                    
                    if (component.Field12.Count == 0)
                    {
                        updateObj.AddClearedField(12);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field13)
                    {
                        var mapObj = obj.AddObject(13);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddFixed64(2, keyValuePair.Value);
                    }
                    
                    if (component.Field13.Count == 0)
                    {
                        updateObj.AddClearedField(13);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field14)
                    {
                        var mapObj = obj.AddObject(14);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddSfixed32(2, keyValuePair.Value);
                    }
                    
                    if (component.Field14.Count == 0)
                    {
                        updateObj.AddClearedField(14);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field15)
                    {
                        var mapObj = obj.AddObject(15);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddSfixed64(2, keyValuePair.Value);
                    }
                    
                    if (component.Field15.Count == 0)
                    {
                        updateObj.AddClearedField(15);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field16)
                    {
                        var mapObj = obj.AddObject(16);
                        mapObj.AddString(1, keyValuePair.Key);
                        mapObj.AddEntityId(2, keyValuePair.Value);
                    }
                    
                    if (component.Field16.Count == 0)
                    {
                        updateObj.AddClearedField(16);
                    }
                    
                }
                {
                    foreach (var keyValuePair in component.Field17)
                    {
                        var mapObj = obj.AddObject(17);
                        mapObj.AddString(1, keyValuePair.Key);
                        global::Improbable.Gdk.Tests.SomeType.Serialization.Serialize(keyValuePair.Value, mapObj.AddObject(2));
                    }
                    
                    if (component.Field17.Count == 0)
                    {
                        updateObj.AddClearedField(17);
                    }
                    
                }
            }

            public static Improbable.Gdk.Tests.ExhaustiveMapValue.Component Deserialize(global::Improbable.Worker.Core.SchemaObject obj, global::Unity.Entities.World world)
            {
                var component = new Improbable.Gdk.Tests.ExhaustiveMapValue.Component();

                component.field1Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field1Provider.Allocate(world);
                {
                    var map = component.Field1 = new global::System.Collections.Generic.Dictionary<string,BlittableBool>();
                    var mapSize = obj.GetObjectCount(1);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetBool(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field2Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field2Provider.Allocate(world);
                {
                    var map = component.Field2 = new global::System.Collections.Generic.Dictionary<string,float>();
                    var mapSize = obj.GetObjectCount(2);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFloat(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field3Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field3Provider.Allocate(world);
                {
                    var map = component.Field3 = new global::System.Collections.Generic.Dictionary<string,byte[]>();
                    var mapSize = obj.GetObjectCount(3);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetBytes(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field4Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field4Provider.Allocate(world);
                {
                    var map = component.Field4 = new global::System.Collections.Generic.Dictionary<string,int>();
                    var mapSize = obj.GetObjectCount(4);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetInt32(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field5Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field5Provider.Allocate(world);
                {
                    var map = component.Field5 = new global::System.Collections.Generic.Dictionary<string,long>();
                    var mapSize = obj.GetObjectCount(5);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetInt64(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field6Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field6Provider.Allocate(world);
                {
                    var map = component.Field6 = new global::System.Collections.Generic.Dictionary<string,double>();
                    var mapSize = obj.GetObjectCount(6);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetDouble(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field7Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field7Provider.Allocate(world);
                {
                    var map = component.Field7 = new global::System.Collections.Generic.Dictionary<string,string>();
                    var mapSize = obj.GetObjectCount(7);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(7, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetString(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field8Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field8Provider.Allocate(world);
                {
                    var map = component.Field8 = new global::System.Collections.Generic.Dictionary<string,uint>();
                    var mapSize = obj.GetObjectCount(8);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetUint32(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field9Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field9Provider.Allocate(world);
                {
                    var map = component.Field9 = new global::System.Collections.Generic.Dictionary<string,ulong>();
                    var mapSize = obj.GetObjectCount(9);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetUint64(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field10Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field10Provider.Allocate(world);
                {
                    var map = component.Field10 = new global::System.Collections.Generic.Dictionary<string,int>();
                    var mapSize = obj.GetObjectCount(10);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSint32(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field11Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field11Provider.Allocate(world);
                {
                    var map = component.Field11 = new global::System.Collections.Generic.Dictionary<string,long>();
                    var mapSize = obj.GetObjectCount(11);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSint64(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field12Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field12Provider.Allocate(world);
                {
                    var map = component.Field12 = new global::System.Collections.Generic.Dictionary<string,uint>();
                    var mapSize = obj.GetObjectCount(12);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFixed32(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field13Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field13Provider.Allocate(world);
                {
                    var map = component.Field13 = new global::System.Collections.Generic.Dictionary<string,ulong>();
                    var mapSize = obj.GetObjectCount(13);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFixed64(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field14Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field14Provider.Allocate(world);
                {
                    var map = component.Field14 = new global::System.Collections.Generic.Dictionary<string,int>();
                    var mapSize = obj.GetObjectCount(14);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSfixed32(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field15Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field15Provider.Allocate(world);
                {
                    var map = component.Field15 = new global::System.Collections.Generic.Dictionary<string,long>();
                    var mapSize = obj.GetObjectCount(15);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSfixed64(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field16Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field16Provider.Allocate(world);
                {
                    var map = component.Field16 = new global::System.Collections.Generic.Dictionary<string,global::Improbable.Worker.EntityId>();
                    var mapSize = obj.GetObjectCount(16);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntityId(2);
                        map.Add(key, value);
                    }
                    
                }
                component.field17Handle = Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.Field17Provider.Allocate(world);
                {
                    var map = component.Field17 = new global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType>();
                    var mapSize = obj.GetObjectCount(17);
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(mapObj.GetObject(2));
                        map.Add(key, value);
                    }
                    
                }
                return component;
            }

            public static Improbable.Gdk.Tests.ExhaustiveMapValue.Update DeserializeUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj)
            {
                var update = new Improbable.Gdk.Tests.ExhaustiveMapValue.Update();
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    var mapSize = obj.GetObjectCount(1);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 1;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field1 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,BlittableBool>>(new global::System.Collections.Generic.Dictionary<string,BlittableBool>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetBool(2);
                        update.Field1.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(2);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field2 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,float>>(new global::System.Collections.Generic.Dictionary<string,float>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFloat(2);
                        update.Field2.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(3);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 3;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field3 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,byte[]>>(new global::System.Collections.Generic.Dictionary<string,byte[]>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetBytes(2);
                        update.Field3.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(4);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 4;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field4 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,int>>(new global::System.Collections.Generic.Dictionary<string,int>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetInt32(2);
                        update.Field4.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(5);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 5;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field5 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,long>>(new global::System.Collections.Generic.Dictionary<string,long>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetInt64(2);
                        update.Field5.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(6);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 6;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field6 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,double>>(new global::System.Collections.Generic.Dictionary<string,double>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetDouble(2);
                        update.Field6.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(7);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 7;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field7 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,string>>(new global::System.Collections.Generic.Dictionary<string,string>());
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 8;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field8 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,uint>>(new global::System.Collections.Generic.Dictionary<string,uint>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetUint32(2);
                        update.Field8.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(9);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 9;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field9 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,ulong>>(new global::System.Collections.Generic.Dictionary<string,ulong>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetUint64(2);
                        update.Field9.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(10);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 10;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field10 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,int>>(new global::System.Collections.Generic.Dictionary<string,int>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSint32(2);
                        update.Field10.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(11);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 11;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field11 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,long>>(new global::System.Collections.Generic.Dictionary<string,long>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSint64(2);
                        update.Field11.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(12);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 12;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field12 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,uint>>(new global::System.Collections.Generic.Dictionary<string,uint>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFixed32(2);
                        update.Field12.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(13);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 13;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field13 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,ulong>>(new global::System.Collections.Generic.Dictionary<string,ulong>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFixed64(2);
                        update.Field13.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(14);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 14;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field14 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,int>>(new global::System.Collections.Generic.Dictionary<string,int>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSfixed32(2);
                        update.Field14.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(15);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 15;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field15 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,long>>(new global::System.Collections.Generic.Dictionary<string,long>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSfixed64(2);
                        update.Field15.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(16);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 16;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field16 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,global::Improbable.Worker.EntityId>>(new global::System.Collections.Generic.Dictionary<string,global::Improbable.Worker.EntityId>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntityId(2);
                        update.Field16.Value.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(17);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 17;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        update.Field17 = new global::Improbable.Gdk.Core.Option<global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType>>(new global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType>());
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(mapObj.GetObject(2));
                        update.Field17.Value.Add(key, value);
                    }
                    
                }
                return update;
            }

            public static void ApplyUpdate(global::Improbable.Worker.Core.SchemaComponentUpdate updateObj, ref Improbable.Gdk.Tests.ExhaustiveMapValue.Component component)
            {
                var obj = updateObj.GetFields();

                var clearedFields = updateObj.GetClearedFields();

                {
                    var mapSize = obj.GetObjectCount(1);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 1;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field1.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(1, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetBool(2);
                        component.Field1.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(2);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 2;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field2.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(2, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFloat(2);
                        component.Field2.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(3);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 3;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field3.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(3, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetBytes(2);
                        component.Field3.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(4);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 4;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field4.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(4, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetInt32(2);
                        component.Field4.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(5);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 5;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field5.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(5, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetInt64(2);
                        component.Field5.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(6);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 6;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field6.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(6, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetDouble(2);
                        component.Field6.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(7);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 7;
                        if (isCleared)
                        {
                            break;
                        }
                    }
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
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 8;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field8.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(8, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetUint32(2);
                        component.Field8.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(9);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 9;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field9.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(9, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetUint64(2);
                        component.Field9.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(10);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 10;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field10.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(10, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSint32(2);
                        component.Field10.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(11);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 11;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field11.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(11, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSint64(2);
                        component.Field11.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(12);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 12;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field12.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(12, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFixed32(2);
                        component.Field12.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(13);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 13;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field13.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(13, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetFixed64(2);
                        component.Field13.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(14);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 14;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field14.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(14, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSfixed32(2);
                        component.Field14.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(15);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 15;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field15.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(15, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetSfixed64(2);
                        component.Field15.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(16);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 16;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field16.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(16, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = mapObj.GetEntityId(2);
                        component.Field16.Add(key, value);
                    }
                    
                }
                {
                    var mapSize = obj.GetObjectCount(17);
                    bool isCleared = false;
                    foreach (var fieldIndex in clearedFields)
                    {
                        isCleared = fieldIndex == 17;
                        if (isCleared)
                        {
                            break;
                        }
                    }
                    if (mapSize > 0 || isCleared)
                    {
                        component.Field17.Clear();
                    }
                    for (var i = 0; i < mapSize; i++)
                    {
                        var mapObj = obj.IndexObject(17, (uint) i);
                        var key = mapObj.GetString(1);
                        var value = global::Improbable.Gdk.Tests.SomeType.Serialization.Deserialize(mapObj.GetObject(2));
                        component.Field17.Add(key, value);
                    }
                    
                }
            }
        }

        public struct Update : ISpatialComponentUpdate
        {
            internal static Stack<List<Update>> Pool = new Stack<List<Update>>();

            public Option<global::System.Collections.Generic.Dictionary<string,BlittableBool>> Field1;
            public Option<global::System.Collections.Generic.Dictionary<string,float>> Field2;
            public Option<global::System.Collections.Generic.Dictionary<string,byte[]>> Field3;
            public Option<global::System.Collections.Generic.Dictionary<string,int>> Field4;
            public Option<global::System.Collections.Generic.Dictionary<string,long>> Field5;
            public Option<global::System.Collections.Generic.Dictionary<string,double>> Field6;
            public Option<global::System.Collections.Generic.Dictionary<string,string>> Field7;
            public Option<global::System.Collections.Generic.Dictionary<string,uint>> Field8;
            public Option<global::System.Collections.Generic.Dictionary<string,ulong>> Field9;
            public Option<global::System.Collections.Generic.Dictionary<string,int>> Field10;
            public Option<global::System.Collections.Generic.Dictionary<string,long>> Field11;
            public Option<global::System.Collections.Generic.Dictionary<string,uint>> Field12;
            public Option<global::System.Collections.Generic.Dictionary<string,ulong>> Field13;
            public Option<global::System.Collections.Generic.Dictionary<string,int>> Field14;
            public Option<global::System.Collections.Generic.Dictionary<string,long>> Field15;
            public Option<global::System.Collections.Generic.Dictionary<string,global::Improbable.Worker.EntityId>> Field16;
            public Option<global::System.Collections.Generic.Dictionary<string,global::Improbable.Gdk.Tests.SomeType>> Field17;
        }

        public struct ReceivedUpdates : IComponentData
        {
            internal uint handle;
            public global::System.Collections.Generic.List<Update> Updates
            {
                get => Improbable.Gdk.Tests.ExhaustiveMapValue.ReferenceTypeProviders.UpdatesProvider.Get(handle);
            }
        }

        internal class ExhaustiveMapValueDynamic : IDynamicInvokable
        {
            public uint ComponentId => ExhaustiveMapValue.ComponentId;

            private static Component DeserializeData(ComponentData data, World world)
            {
                var schemaDataOpt = data.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentData)}");
                }

                return Serialization.Deserialize(schemaDataOpt.Value.GetFields(), world);
            }

            private static Update DeserializeUpdate(ComponentUpdate update, World world)
            {
                var schemaDataOpt = update.SchemaData;
                if (!schemaDataOpt.HasValue)
                {
                    throw new ArgumentException($"Can not deserialize an empty {nameof(ComponentUpdate)}");
                }

                return Serialization.DeserializeUpdate(schemaDataOpt.Value);
            }

            public void InvokeHandler(Dynamic.IHandler handler)
            {
                handler.Accept<Component, Update>(ExhaustiveMapValue.ComponentId, DeserializeData, DeserializeUpdate);
            }
        }
    }
}
