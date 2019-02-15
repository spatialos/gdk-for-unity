using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    public interface IComponentDiffStorage
    {
        uint GetComponentId();

        void Clear();
        void RemoveEntityComponent(long entityId);
    }

    public interface ICommandDiffStorage
    {
        uint GetComponentId();
        uint GetCommandId();

        void Clear();
        void RemoveRequests(long entityId);
    }

    public interface IDiffComponentAddedStorage<T> where T : ISpatialComponentUpdate
    {
        void AddEntityComponent(long entityId, T component);
    }

    public interface IDiffAuthorityStorage : IComponentDiffStorage
    {
        void AddAuthorityChange(AuthorityChangeReceived authorityChange);
    }

    public interface IDiffUpdateStorage<T> : IComponentDiffStorage
        where T : ISpatialComponentUpdate
    {
        void AddUpdate(ComponentUpdateReceived<T> update);
    }

    public interface IDiffEventStorage<T> : IComponentDiffStorage
        where T : IEvent
    {
        void AddEvent(ComponentEventReceived<T> ev);
    }

    public interface IDiffCommandRequestStorage<T> : ICommandDiffStorage
        where T : IReceivedCommandRequest
    {
        void AddRequest(T request);
    }

    public interface IDiffCommandResponseStorage<T> : ICommandDiffStorage
        where T : IRawReceivedCommandResponse
    {
        void AddResponse(T response);
    }
}
