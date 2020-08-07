using System;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    public sealed class IOStorage : IDisposable
    {
        private readonly CIO.StorageHandle storage;

        public IOStorage()
        {
            storage = CIO.StorageCreate();
        }

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            storage.Dispose();
        }

        public void Clear()
        {
            CIO.StorageClear(storage);
        }
    }
}
