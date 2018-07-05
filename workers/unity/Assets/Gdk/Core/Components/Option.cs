using System;

namespace Improbable.Gdk.Core
{
    public struct Option<T> : IEquatable<Option<T>>
    {
        public static readonly Option<T> Empty = new Option<T>();

        public BlittableBool HasValue { get; }

        private readonly T value;

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new CalledValueOnEmptyOptionException("Called Value on empty Option.");
                }

                return value;
            }
        }

        public Option(T value)
        {
            if (!typeof(T).IsValueType && value == null)
            {
                throw new CreatedOptionWithNullPayloadException("Options may not have null payloads.");
            }

            HasValue = true;
            this.value = value;
        }

        public bool TryGetValue(out T value)
        {
            if (!HasValue)
            {
                value = default(T);
                return false;
            }

            value = this.value;
            return true;
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ? value : defaultValue;
        }

        public override bool Equals(object other)
        {
            return other is Option<T> && this.Equals((Option<T>) other);
        }

        public bool Equals(Option<T> other)
        {
            if (!HasValue && !other.HasValue)
            {
                return true;
            }

            if (HasValue && other.HasValue)
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
            return HasValue ? value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return HasValue ? $"Option[{value.ToString()}]" : "Option.Empty";
        }

        public static implicit operator Option<T>(T value)
        {
            return new Option<T>(value);
        }

        public static implicit operator T(Option<T> option)
        {
            return option.Value;
        }
    }

    public class CreatedOptionWithNullPayloadException : Exception
    {
        public CreatedOptionWithNullPayloadException(string message)
            : base(message)
        {
        }
    }

    public class CalledValueOnEmptyOptionException : Exception
    {
        public CalledValueOnEmptyOptionException(string message)
            : base(message)
        {
        }
    }
}
