using Improbable.Worker;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    internal interface IReaderInternal
    {
        void OnAuthorityChange(Authority authority);
        void OnComponentUpdate(object update);
        void OnEvent(int eventIndex, object payload);
        void OnCommandRequest(int commandIndex, object commandRequest);
    }
}