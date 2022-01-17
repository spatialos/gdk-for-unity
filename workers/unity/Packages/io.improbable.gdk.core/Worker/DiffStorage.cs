using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    public interface IComponentDiffStorage
    {
        Type[] GetEventTypes();
        Type GetUpdateType();

        bool Dirty { get; }

        void Clear();
        void RemoveEntityComponent(long entityId);

        HashSet<EntityId> GetComponentsAdded();
        HashSet<EntityId> GetComponentsRemoved();
    }

    public interface ICommandDiffStorage
    {
        bool Dirty { get; }
        void Clear();
    }

    public interface IComponentCommandDiffStorage : ICommandDiffStorage
    {
        uint ComponentId { get; }
        uint CommandId { get; }

        Type RequestType { get; }
        Type ResponseType { get; }

        void RemoveRequests(long entityId);
    }

    public interface IDiffComponentAddedStorage<T> where T : ISpatialComponentUpdate
    {
        void AddEntityComponent(long entityId, T component, uint updateId);
    }

    public interface IDiffAuthorityStorage : IComponentDiffStorage
    {
        void AddAuthorityChange(AuthorityChangeReceived authorityChange);
        MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges();
        MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges(EntityId entityId);
    }

    public interface IDiffUpdateStorage<T> : IComponentDiffStorage
        where T : ISpatialComponentUpdate
    {
        void AddUpdate(ComponentUpdateReceived<T> update);
        MessagesSpan<ComponentUpdateReceived<T>> GetUpdates();
        MessagesSpan<ComponentUpdateReceived<T>> GetUpdates(EntityId entityId);
    }

    public interface IDiffEventStorage<T> : IComponentDiffStorage
        where T : IEvent
    {
        void AddEvent(ComponentEventReceived<T> ev);
        MessagesSpan<ComponentEventReceived<T>> GetEvents();
        MessagesSpan<ComponentEventReceived<T>> GetEvents(EntityId entityId);
    }

    public interface IDiffCommandRequestStorage<T> : ICommandDiffStorage
        where T : struct, IReceivedCommandRequest
    {
        void AddRequest(T request);
        MessagesSpan<T> GetRequests();
        MessagesSpan<T> GetRequests(EntityId targetEntityId);
    }

    public interface IDiffCommandResponseStorage<T> : ICommandDiffStorage
        where T : struct, IReceivedCommandResponse
    {
        void AddResponse(T response);
        MessagesSpan<T> GetResponses();
        T? GetResponse(CommandRequestId requestId);
    }
}
