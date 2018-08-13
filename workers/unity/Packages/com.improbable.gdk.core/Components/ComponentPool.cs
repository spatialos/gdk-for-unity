using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Components
{
    public class ComponentPool<T>
    {
        public delegate TComponent InitFunc<out TComponent>();

        public delegate void ResetFunc<in TComponent>(TComponent component);

        private readonly Stack<T> pool = new Stack<T>();
        private readonly InitFunc<T> init;
        private readonly ResetFunc<T> reset;

        public ComponentPool(InitFunc<T> initFunc, ResetFunc<T> resetFunc)
        {
            init = initFunc ?? throw new ArgumentNullException(nameof(initFunc));
            reset = resetFunc ?? throw new ArgumentNullException(nameof(resetFunc));
        }

        public T GetComponent()
        {
            return pool.Count > 0 ? pool.Pop() : init();
        }

        public void PutComponent(T component)
        {
            reset(component);
            pool.Push(component);
        }
    }
}
