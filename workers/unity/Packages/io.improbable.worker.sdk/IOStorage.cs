using System;
using System.Runtime.InteropServices;
using Improbable.Worker.CInterop.Internal;
using Packages.io.improbable.worker.sdk;

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
        }

        public void Clear()
        {
            CIO.StorageClear(storage);
        }
    }
}
