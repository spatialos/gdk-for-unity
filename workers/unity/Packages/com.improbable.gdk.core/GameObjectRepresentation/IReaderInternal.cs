using System.Runtime.CompilerServices;
using Improbable.Worker;
using Unity.Entities;

[assembly:InternalsVisibleTo("Improbable.Gdk.Core.EditmodeTests")]

namespace Improbable.Gdk.Core.MonoBehaviours
{
    internal interface IReaderInternal<TComponent, in TComponentUpdate>
        where TComponent : ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TComponent>
    {
        void OnAuthorityChange(Authority authority);
        void OnComponentUpdate(TComponentUpdate update);
        void OnEvent<TEvent>(int eventIndex, TEvent payload);
        void OnCommandRequest<TCommandRequest>(int commandIndex, TCommandRequest commandRequest);
    }
}
