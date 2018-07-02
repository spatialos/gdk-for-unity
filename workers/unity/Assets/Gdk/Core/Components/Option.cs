using System;
using Unity.Mathematics;

namespace Improbable.Gdk.Core
{
    public struct Option<T> : IEquatable<Option<T>>
    {
        public bool1 HasValue { get; private set; }

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException("Called Value on empty Option.");
                }

                return Value;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("newValue is null.");
                }

                HasValue = true;
                Value = value;
            }
        }

        public Option(T newValue)
        {
            HasValue = false;
            Value = newValue;
        }

        public void Clear()
        {
            HasValue = false;
            Value = default(T);
        }

        public bool TryGetValue(out T value)
        {
            if (!HasValue)
            {
                value = default(T);
                return false;
            } 

            value = Value;
            return true;
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ? Value : defaultValue;
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
            if (!HasValue && !other.HasValue)
            {
                return true;
            }

            if (HasValue && other.HasValue)
            {
                return Value.Equals(other.Value);
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
            return HasValue ? Value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return HasValue ? $"Option[{Value.ToString()}]" : "Option.Empty";
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
