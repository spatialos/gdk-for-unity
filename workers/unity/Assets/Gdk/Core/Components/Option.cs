using System;
using Unity.Mathematics;

namespace Improbable.Gdk.Core
{
    public struct Option<T> : IEquatable<Option<T>>
    {
        private bool hasValue;
        private T value;
        
        public Option(T newValue)
        {
            hasValue = false;
            value = default(T);
            Value = newValue;
        }

        public bool1 HasValue => hasValue;

        public T Value
        {
            get
            {
                if (!hasValue)
                {
                    throw new InvalidOperationException("Called Value on empty Option.");
                }

                return value;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("newValue is null.");
                }

                hasValue = true;
                this.value = value;
            }
        }

        public void Clear()
        {
            hasValue = false;
            value = default(T);
        }

        public bool TryGetValue(out T value)
        {
            if (!hasValue)
            {
                value = default(T);
                return false;
            } 

            value = this.value;
            return true;
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return hasValue ? value : defaultValue;
        }

        public override bool Equals(object other)
        {
            if (other is Option<T>)
            {
                return this.Equals((Option<T>)other);
            }

            return false;
        }

        public bool Equals(Option<T> other)
        {
            if (!hasValue && !other.HasValue)
            {
                return true;
            }

            if (hasValue && other.HasValue)
            {
                return value.Equals(other.Value);
            }

            return false;
        }

        public static bool operator ==(Option<T> a, Option<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Option<T> a, Option<T> b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return hasValue ? value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return hasValue ? $"Option[{value.ToString()}]" : "Option.Empty";
        }

        public static implicit operator Option<T>(T value)
        {
            return new Option<T>(value);
        }

        public static explicit operator T(Option<T> option)
        {
            return option.Value;
        }
    }
}
