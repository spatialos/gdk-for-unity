using System;
using UnityEngine;

namespace Improbable.Gdk.Legacy.BuildSystem.Configuration
{
    [Serializable]
    public struct WorkerPlatform : IEquatable<WorkerPlatform>
    {
        [SerializeField] private string name;
        public static readonly WorkerPlatform UnityClient = new WorkerPlatform("UnityClient");
        public static readonly WorkerPlatform UnityGameLogic = new WorkerPlatform("UnityGameLogic");

        public WorkerPlatform(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        public static bool operator ==(WorkerPlatform self, WorkerPlatform other)
        {
            return self.name == other.name;
        }

        public static bool operator !=(WorkerPlatform self, WorkerPlatform other)
        {
            return self.name != other.name;
        }

        public bool Equals(WorkerPlatform other)
        {
            return string.Equals(name, other.name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is WorkerPlatform && Equals((WorkerPlatform) obj);
        }

        public override int GetHashCode()
        {
            return name != null ? name.GetHashCode() : 0;
        }
    }
}
