using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface IConnectionHandlerBuilder
    {
        Task<IConnectionHandler> CreateAsync(CancellationToken? token = null);
        string WorkerType { get; }
    }
}
