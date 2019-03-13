using System.Threading.Tasks;

namespace Improbable.Gdk.Core
{
    public interface IConnectionHandlerBuilder
    {
        Task<IConnectionHandler> CreateAsync();
    }
}
