using System.Collections.Generic;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public struct ComponentAdded<T> : IComponentData where T : ISpatialComponentData
    {
    }

    public struct ComponentRemoved<T> : IComponentData where T : ISpatialComponentData
    {
    }
}
