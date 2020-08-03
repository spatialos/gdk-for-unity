using System;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    public class IOStorage : IDisposable
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
            GC.SuppressFinalize(this);
        }

        public void Clear()
        {
            CIO.StorageClear(storage);
        }
    }
}
