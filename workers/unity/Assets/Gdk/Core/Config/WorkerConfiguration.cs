using System;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [Serializable]
    public class WorkerConfiguration
    {
        public Vector3 Origin;
        public bool IsExpanded;
        public bool IsEnabled;
        public int TypeIndex;
        public string Name;
        public string Type;

        public WorkerConfiguration(string type)
        {
            IsEnabled = true;
            Type = type;
            Name = Type;
        }
    }
}
