using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Intermediate object for building a <see cref="IConnectionHandler"/> object.
    /// </summary>
    public interface IConnectionHandlerBuilder
    {
        /// <summary>
        ///     Creates a <see cref="IConnectionHandler"/> asynchronously.
        /// </summary>
        /// <param name="token">A cancellation token which should cancel the underlying connection attempt.</param>
        /// <returns>A task that represents the asynchronous creation of the connection handler object.</returns>
        Task<IConnectionHandler> CreateAsync(CancellationToken? token = null);

        /// <summary>
        ///     The type of worker that the resulting connection handler will represent.
        /// </summary>
        string WorkerType { get; }
    }
}
