using System;
using Improbable.Worker.CInterop.Internal;

namespace Improbable.Worker.CInterop
{
    public sealed class IOStorage : IDisposable
    {
        internal readonly CIO.StorageHandle Storage;

        public IOStorage()
        {
            Storage = CIO.StorageCreate();
        }

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            Storage.Dispose();
        }

        public void Clear()
        {
            CIO.StorageClear(Storage);
        }
    }
}
