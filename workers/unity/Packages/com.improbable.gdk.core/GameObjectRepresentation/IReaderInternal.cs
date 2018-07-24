using System.Runtime.CompilerServices;
using Improbable.Worker;
using Unity.Entities;

[assembly: InternalsVisibleTo("Improbable.Gdk.Core.EditmodeTests")]

namespace Improbable.Gdk.Core.MonoBehaviours
{
    internal interface IReaderInternal
    {
        void OnAuthorityChange(Authority authority);
        void OnComponentUpdate();
        void OnEvent(int eventIndex);
        void OnCommandRequest(int commandIndex);
    }
}
