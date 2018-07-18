// Generated by SpatialOS codegen. DO NOT EDIT!
// source: improbable.gdk.tests.ExhaustiveOptionalData in improbable/gdk/tests/exhaustive_test.schema.

namespace Improbable.Gdk.Tests
{

public partial struct ExhaustiveOptionalData : global::System.IEquatable<ExhaustiveOptionalData>, global::Improbable.Collections.IDeepCopyable<ExhaustiveOptionalData>
{
  /// <summary>
  /// Field field2 = 2.
  /// </summary>
  public global::Improbable.Collections.Option<float> field2;
  /// <summary>
  /// Field field4 = 4.
  /// </summary>
  public global::Improbable.Collections.Option<int> field4;
  /// <summary>
  /// Field field5 = 5.
  /// </summary>
  public global::Improbable.Collections.Option<long> field5;
  /// <summary>
  /// Field field6 = 6.
  /// </summary>
  public global::Improbable.Collections.Option<double> field6;
  /// <summary>
  /// Field field8 = 8.
  /// </summary>
  public global::Improbable.Collections.Option<uint> field8;
  /// <summary>
  /// Field field9 = 9.
  /// </summary>
  public global::Improbable.Collections.Option<ulong> field9;
  /// <summary>
  /// Field field10 = 10.
  /// </summary>
  public global::Improbable.Collections.Option<int> field10;
  /// <summary>
  /// Field field11 = 11.
  /// </summary>
  public global::Improbable.Collections.Option<long> field11;
  /// <summary>
  /// Field field12 = 12.
  /// </summary>
  public global::Improbable.Collections.Option<uint> field12;
  /// <summary>
  /// Field field13 = 13.
  /// </summary>
  public global::Improbable.Collections.Option<ulong> field13;
  /// <summary>
  /// Field field14 = 14.
  /// </summary>
  public global::Improbable.Collections.Option<int> field14;
  /// <summary>
  /// Field field15 = 15.
  /// </summary>
  public global::Improbable.Collections.Option<long> field15;
  /// <summary>
  /// Field field16 = 16.
  /// </summary>
  public global::Improbable.Collections.Option<global::Improbable.EntityId> field16;
  /// <summary>
  /// Field field17 = 17.
  /// </summary>
  public global::Improbable.Collections.Option<global::Improbable.Gdk.Tests.SomeType> field17;

  public ExhaustiveOptionalData(
      global::Improbable.Collections.Option<float> field2,
      global::Improbable.Collections.Option<int> field4,
      global::Improbable.Collections.Option<long> field5,
      global::Improbable.Collections.Option<double> field6,
      global::Improbable.Collections.Option<uint> field8,
      global::Improbable.Collections.Option<ulong> field9,
      global::Improbable.Collections.Option<int> field10,
      global::Improbable.Collections.Option<long> field11,
      global::Improbable.Collections.Option<uint> field12,
      global::Improbable.Collections.Option<ulong> field13,
      global::Improbable.Collections.Option<int> field14,
      global::Improbable.Collections.Option<long> field15,
      global::Improbable.Collections.Option<global::Improbable.EntityId> field16,
      global::Improbable.Collections.Option<global::Improbable.Gdk.Tests.SomeType> field17)
  {
    this.field2 = field2;
    this.field4 = field4;
    this.field5 = field5;
    this.field6 = field6;
    this.field8 = field8;
    this.field9 = field9;
    this.field10 = field10;
    this.field11 = field11;
    this.field12 = field12;
    this.field13 = field13;
    this.field14 = field14;
    this.field15 = field15;
    this.field16 = field16;
    this.field17 = field17;
  }

  public static ExhaustiveOptionalData Create()
  {
    var _result = new ExhaustiveOptionalData();
    return _result;
  }

  public ExhaustiveOptionalData DeepCopy()
  {
    var _result = new ExhaustiveOptionalData();
    _result.field2 = field2.DeepCopy();
    _result.field4 = field4.DeepCopy();
    _result.field5 = field5.DeepCopy();
    _result.field6 = field6.DeepCopy();
    _result.field8 = field8.DeepCopy();
    _result.field9 = field9.DeepCopy();
    _result.field10 = field10.DeepCopy();
    _result.field11 = field11.DeepCopy();
    _result.field12 = field12.DeepCopy();
    _result.field13 = field13.DeepCopy();
    _result.field14 = field14.DeepCopy();
    _result.field15 = field15.DeepCopy();
    _result.field16 = field16.DeepCopy();
    _result.field17 = field17.DeepCopy();
    return _result;

  }

  public override bool Equals(object _obj)
  {
    return _obj is ExhaustiveOptionalData && Equals((ExhaustiveOptionalData) _obj);
  }

  public static bool operator==(ExhaustiveOptionalData a, ExhaustiveOptionalData b)
  {
    return a.Equals(b);
  }

  public static bool operator!=(ExhaustiveOptionalData a, ExhaustiveOptionalData b)
  {
    return !a.Equals(b);
  }

  public bool Equals(ExhaustiveOptionalData _obj)
  {
    return
        field2 == _obj.field2 &&
        field4 == _obj.field4 &&
        field5 == _obj.field5 &&
        field6 == _obj.field6 &&
        field8 == _obj.field8 &&
        field9 == _obj.field9 &&
        field10 == _obj.field10 &&
        field11 == _obj.field11 &&
        field12 == _obj.field12 &&
        field13 == _obj.field13 &&
        field14 == _obj.field14 &&
        field15 == _obj.field15 &&
        field16 == _obj.field16 &&
        field17 == _obj.field17;
  }

  public override int GetHashCode()
  {
    int _result = 1327;
    _result = (_result * 977) + field2.GetHashCode();
    _result = (_result * 977) + field4.GetHashCode();
    _result = (_result * 977) + field5.GetHashCode();
    _result = (_result * 977) + field6.GetHashCode();
    _result = (_result * 977) + field8.GetHashCode();
    _result = (_result * 977) + field9.GetHashCode();
    _result = (_result * 977) + field10.GetHashCode();
    _result = (_result * 977) + field11.GetHashCode();
    _result = (_result * 977) + field12.GetHashCode();
    _result = (_result * 977) + field13.GetHashCode();
    _result = (_result * 977) + field14.GetHashCode();
    _result = (_result * 977) + field15.GetHashCode();
    _result = (_result * 977) + field16.GetHashCode();
    _result = (_result * 977) + field17.GetHashCode();
    return _result;
  }
}

public static class ExhaustiveOptionalData_Internal
{
  public static unsafe void Write(global::Improbable.Worker.Internal.GcHandlePool _pool,
                                  ExhaustiveOptionalData _data, global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
    if (_data.field2.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddFloat(_obj, 2, _data.field2.Value);
    }
    if (_data.field4.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddInt32(_obj, 4, _data.field4.Value);
    }
    if (_data.field5.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddInt64(_obj, 5, _data.field5.Value);
    }
    if (_data.field6.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddDouble(_obj, 6, _data.field6.Value);
    }
    if (_data.field8.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddUint32(_obj, 8, _data.field8.Value);
    }
    if (_data.field9.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddUint64(_obj, 9, _data.field9.Value);
    }
    if (_data.field10.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddSint32(_obj, 10, _data.field10.Value);
    }
    if (_data.field11.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddSint64(_obj, 11, _data.field11.Value);
    }
    if (_data.field12.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddFixed32(_obj, 12, _data.field12.Value);
    }
    if (_data.field13.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddFixed64(_obj, 13, _data.field13.Value);
    }
    if (_data.field14.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddSfixed32(_obj, 14, _data.field14.Value);
    }
    if (_data.field15.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddSfixed64(_obj, 15, _data.field15.Value);
    }
    if (_data.field16.HasValue)
    {
      global::Improbable.Worker.Internal.Pbio.AddInt64(_obj, 16, _data.field16.Value.Id);
    }
    if (_data.field17.HasValue)
    {
      global::Improbable.Gdk.Tests.SomeType_Internal.Write(_pool, _data.field17.Value, global::Improbable.Worker.Internal.Pbio.AddObject(_obj, 17));
    }
  }

  public static unsafe ExhaustiveOptionalData Read(global::Improbable.Worker.Internal.Pbio.Object* _obj)
  {
    ExhaustiveOptionalData _data;
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetFloatCount(_obj, 2);
      if (_count > 0)
      {
        _data.field2 = new global::Improbable.Collections.Option<float>(global::Improbable.Worker.Internal.Pbio.GetFloat(_obj, 2));
      }
      else
      {
        _data.field2 = new global::Improbable.Collections.Option<float>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetInt32Count(_obj, 4);
      if (_count > 0)
      {
        _data.field4 = new global::Improbable.Collections.Option<int>(global::Improbable.Worker.Internal.Pbio.GetInt32(_obj, 4));
      }
      else
      {
        _data.field4 = new global::Improbable.Collections.Option<int>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetInt64Count(_obj, 5);
      if (_count > 0)
      {
        _data.field5 = new global::Improbable.Collections.Option<long>(global::Improbable.Worker.Internal.Pbio.GetInt64(_obj, 5));
      }
      else
      {
        _data.field5 = new global::Improbable.Collections.Option<long>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetDoubleCount(_obj, 6);
      if (_count > 0)
      {
        _data.field6 = new global::Improbable.Collections.Option<double>(global::Improbable.Worker.Internal.Pbio.GetDouble(_obj, 6));
      }
      else
      {
        _data.field6 = new global::Improbable.Collections.Option<double>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetUint32Count(_obj, 8);
      if (_count > 0)
      {
        _data.field8 = new global::Improbable.Collections.Option<uint>(global::Improbable.Worker.Internal.Pbio.GetUint32(_obj, 8));
      }
      else
      {
        _data.field8 = new global::Improbable.Collections.Option<uint>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetUint64Count(_obj, 9);
      if (_count > 0)
      {
        _data.field9 = new global::Improbable.Collections.Option<ulong>(global::Improbable.Worker.Internal.Pbio.GetUint64(_obj, 9));
      }
      else
      {
        _data.field9 = new global::Improbable.Collections.Option<ulong>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetSint32Count(_obj, 10);
      if (_count > 0)
      {
        _data.field10 = new global::Improbable.Collections.Option<int>(global::Improbable.Worker.Internal.Pbio.GetSint32(_obj, 10));
      }
      else
      {
        _data.field10 = new global::Improbable.Collections.Option<int>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetSint64Count(_obj, 11);
      if (_count > 0)
      {
        _data.field11 = new global::Improbable.Collections.Option<long>(global::Improbable.Worker.Internal.Pbio.GetSint64(_obj, 11));
      }
      else
      {
        _data.field11 = new global::Improbable.Collections.Option<long>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetFixed32Count(_obj, 12);
      if (_count > 0)
      {
        _data.field12 = new global::Improbable.Collections.Option<uint>(global::Improbable.Worker.Internal.Pbio.GetFixed32(_obj, 12));
      }
      else
      {
        _data.field12 = new global::Improbable.Collections.Option<uint>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetFixed64Count(_obj, 13);
      if (_count > 0)
      {
        _data.field13 = new global::Improbable.Collections.Option<ulong>(global::Improbable.Worker.Internal.Pbio.GetFixed64(_obj, 13));
      }
      else
      {
        _data.field13 = new global::Improbable.Collections.Option<ulong>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetSfixed32Count(_obj, 14);
      if (_count > 0)
      {
        _data.field14 = new global::Improbable.Collections.Option<int>(global::Improbable.Worker.Internal.Pbio.GetSfixed32(_obj, 14));
      }
      else
      {
        _data.field14 = new global::Improbable.Collections.Option<int>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetSfixed64Count(_obj, 15);
      if (_count > 0)
      {
        _data.field15 = new global::Improbable.Collections.Option<long>(global::Improbable.Worker.Internal.Pbio.GetSfixed64(_obj, 15));
      }
      else
      {
        _data.field15 = new global::Improbable.Collections.Option<long>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetInt64Count(_obj, 16);
      if (_count > 0)
      {
        _data.field16 = new global::Improbable.Collections.Option<global::Improbable.EntityId>(new global::Improbable.EntityId(global::Improbable.Worker.Internal.Pbio.GetInt64(_obj, 16)));
      }
      else
      {
        _data.field16 = new global::Improbable.Collections.Option<global::Improbable.EntityId>();
      }
    }
    {
      var _count = global::Improbable.Worker.Internal.Pbio.GetObjectCount(_obj, 17);
      if (_count > 0)
      {
        _data.field17 = new global::Improbable.Collections.Option<global::Improbable.Gdk.Tests.SomeType>(global::Improbable.Gdk.Tests.SomeType_Internal.Read(global::Improbable.Worker.Internal.Pbio.GetObject(_obj, 17)));
      }
      else
      {
        _data.field17 = new global::Improbable.Collections.Option<global::Improbable.Gdk.Tests.SomeType>();
      }
    }
    return _data;
  }
}

}
